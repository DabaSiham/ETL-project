using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace ETL_role
{
    public class DatabaseHelper
    {
        private string connectionString = "server=localhost;database=postgres;port=5432;username=postgres;password=12345";
        
        public void CreerTablesSiNecessaire()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS articles (
                        id SERIAL PRIMARY KEY,
                        libelle VARCHAR(255) NOT NULL,
                        pu DECIMAL NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS achats (
                        num SERIAL PRIMARY KEY,
                        id INT NOT NULL,
                        qte INT NOT NULL,
                        date DATE NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS ventes (
                        num SERIAL PRIMARY KEY,
                        id INT NOT NULL,
                        qte INT NOT NULL,
                        date DATE NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Bilan (
                        num SERIAL PRIMARY KEY,
                        ArticleID INT,
                        Libelle VARCHAR(100),
                        PrixUnitaire DECIMAL(10, 2),
                        Mois INT,
                        Annee INT,
                        TotalAchats INT,
                        TotalVentes INT,
                        StockRestant INT
                    );                    ";
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }
        

        public void SauvegarderArticles(List<Article> articles)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                foreach (var article in articles)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        cmd.CommandText = "INSERT INTO articles (libelle, pu) VALUES (@Libelle, @PU)";
                        cmd.Parameters.AddWithValue("Libelle", article.Libelle);
                        cmd.Parameters.AddWithValue("PU", article.PU);
                        cmd.ExecuteNonQuery();

                    }
                }
            }
        }

        public void SauvegarderAchats(List<Achat> achats)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                foreach (var achat in achats)
                {
                    try
                    {
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "INSERT INTO achats (id, qte, date) VALUES (@Id, @Qte, @Date)";
                            cmd.Parameters.AddWithValue("Id", achat.Id);
                            cmd.Parameters.AddWithValue("Qte", achat.Qte);
                            cmd.Parameters.AddWithValue("Date", achat.Date);

                            // Vérifier si la connexion est ouverte
                            if (conn.State == ConnectionState.Open)
                            {
                                // Exécuter la commande
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                // Gérer le cas où la connexion est fermée ou null
                                Console.WriteLine("Erreur : La connexion PostgreSQL est fermée ou null.");
                            }
                        }
                    }
                    catch (Npgsql.PostgresException ex)
                    {
                        // Gestion des erreurs
                        // Capturez et affichez les détails de l'erreur

                        Console.WriteLine($"Erreur lors de l'insertion de l'article : {ex.Message}");
                        Console.WriteLine($"Npgsql Error Code: {ex.ErrorCode}");
                        Console.WriteLine($"Error Message: {ex.Message}");
                        Console.WriteLine($"Detail: {ex.Detail}");
                        // Autres informations spécifiques à l'erreur

                    }
                }
            }
        }

        public void SauvegarderVentes(List<Vente> ventes)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                foreach (var vente in ventes)
                {
                    try
                    {
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "INSERT INTO ventes (id, qte, date) VALUES (@Id, @Qte, @Date)";

                            cmd.Parameters.AddWithValue("Id", vente.Id);
                            cmd.Parameters.AddWithValue("Qte", vente.Qte);
                            cmd.Parameters.AddWithValue("Date", vente.Date);

                            cmd.ExecuteNonQuery(); // Utilisation de ExecuteNonQuery pour l'insertion

                            // Pas besoin de récupérer l'ID généré ici car c'est une insertion
                        }
                    }
                    catch (Npgsql.PostgresException ex)
                    {
                        // Gestion des erreurs
                        // Capturez et affichez les détails de l'erreur
                        Console.WriteLine($"Erreur lors de l'insertion de la vente : {ex.Message}");
                        Console.WriteLine($"Npgsql Error Code: {ex.ErrorCode}");
                        Console.WriteLine($"Error Message: {ex.Message}");
                        Console.WriteLine($"Detail: {ex.Detail}");
                        // Autres informations spécifiques à l'erreur
                    }
                }
            }
        }


        public void InsererDonneesBilan()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                            INSERT INTO Bilan (ArticleID, Libelle, PrixUnitaire, Mois, Annee, TotalAchats, TotalVentes, StockRestant)
                            SELECT
                                articles.id AS ArticleID,
                                articles.libelle AS Libelle,
                                articles.pu AS PrixUnitaire,
                                EXTRACT(MONTH FROM COALESCE(achats.month, ventes.month)) AS Mois,
                                EXTRACT(YEAR FROM COALESCE(achats.month, ventes.month)) AS Annee,
                                COALESCE(SUM(achats.qte), 0) AS TotalAchats,
                                COALESCE(SUM(ventes.qte), 0) AS TotalVentes,
                                COALESCE(SUM(achats.qte), 0) - COALESCE(SUM(ventes.qte), 0) AS StockRestant
                            FROM
                                articles
                            LEFT JOIN
                                (SELECT id, DATE_TRUNC('month', date) AS month, SUM(qte) AS qte FROM achats GROUP BY id, month) AS achats ON articles.id = achats.id
                            LEFT JOIN
                                (SELECT id, DATE_TRUNC('month', date) AS month, SUM(qte) AS qte FROM ventes GROUP BY id, month) AS ventes ON articles.id = ventes.id AND achats.month = ventes.month
                            GROUP BY
                                articles.id, articles.libelle, articles.pu,
                                EXTRACT(MONTH FROM COALESCE(achats.month, ventes.month)),
                                EXTRACT(YEAR FROM COALESCE(achats.month, ventes.month))
                            ORDER BY
                                ArticleID, Annee, Mois;

                            ";
                    cmd.ExecuteNonQuery();
                }
            }
        }





        public DataTable GetBilanData()
        {
            DataTable dataTable = new DataTable();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Bilan";
                    using (var reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }
    }
}



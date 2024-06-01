using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExcelDataReader;
using Npgsql;

namespace ETL_role
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Déclarations des variables pour les tables
        DataTable tableAchats;
        DataTable tableVentes;
        DataTable tableArticles;

        private void btnLireExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // Changer à false pour un seul fichier
            openFileDialog.Filter = "Fichiers Excel|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DatabaseHelper dbHelper = new DatabaseHelper();

                // Créer les tables si elles n'existent pas
                dbHelper.CreerTablesSiNecessaire();

                string fichier = openFileDialog.FileName;
                int totalValeursNulles = 0;

                LireFeuillesExcel(fichier, dbHelper, ref totalValeursNulles);

                // Insérer les données dans la table Bilan
                dbHelper.InsererDonneesBilan();

                // Mettre à jour la DataGridView avec les données du Bilan
                dataGridView1_CellContentClick(this, new DataGridViewCellEventArgs(0, 0));

                // Afficher le nombre de valeurs nulles
                lblMessage.Text = $"Nombre de valeurs nulles: {totalValeursNulles}";
            }
        }

        private void LireFeuillesExcel(string fichier, DatabaseHelper dbHelper, ref int totalValeursNulles)
        {
            using (var stream = File.Open(fichier, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });

                    foreach (DataTable table in dataSet.Tables)
                    {
                        if (table.TableName.Equals("Achats", StringComparison.OrdinalIgnoreCase))
                        {
                            tableAchats = table;
                            int valeursNulles;
                            List<Achat> achats = LireTableauAchat(table);
                            achats = TransformerAchats(achats, out valeursNulles);  // Transformation
                            totalValeursNulles += valeursNulles;
                            dbHelper.SauvegarderAchats(achats);
                        }
                        else if (table.TableName.Equals("Ventes", StringComparison.OrdinalIgnoreCase))
                        {
                            tableVentes = table;
                            int valeursNulles;
                            List<Vente> ventes = LireTableauVente(table);
                            ventes = TransformerVentes(ventes, out valeursNulles);  // Transformation
                            totalValeursNulles += valeursNulles;
                            dbHelper.SauvegarderVentes(ventes);
                        }
                        else if (table.TableName.Equals("Articles", StringComparison.OrdinalIgnoreCase))
                        {
                            tableArticles = table;
                            int valeursNulles;
                            List<Article> articles = LireTableauArticle(table);
                            articles = TransformerArticles(articles, out valeursNulles);  // Transformation
                            totalValeursNulles += valeursNulles;
                            dbHelper.SauvegarderArticles(articles);
                        }
                    }
                }
            }
        }



        private List<Achat> TransformerAchats(List<Achat> achats, out int valeursNulles)
        {
            List<Achat> achatsTransformes = new List<Achat>();
            valeursNulles = 0;

            foreach (var achat in achats)
            {
                int nullCount = 0;
                if (achat.Id <= 0) nullCount++;
                if (achat.Qte <= 0) nullCount++;
                if (achat.Date == DateTime.MinValue) nullCount++;

                valeursNulles += nullCount;

                if (nullCount == 0)
                {
                    achatsTransformes.Add(achat);
                }
            }

            return achatsTransformes.Distinct().ToList();
        }

        private List<Vente> TransformerVentes(List<Vente> ventes, out int valeursNulles)
        {
            List<Vente> ventesTransformees = new List<Vente>();
            valeursNulles = 0;

            foreach (var vente in ventes)
            {
                int nullCount = 0;
                if (vente.Id <= 0) nullCount++;
                if (vente.Qte <= 0) nullCount++;
                if (vente.Date == DateTime.MinValue) nullCount++;

                valeursNulles += nullCount;

                if (nullCount == 0)
                {
                    ventesTransformees.Add(vente);
                }
            }

            return ventesTransformees.Distinct().ToList();
        }

        private List<Article> TransformerArticles(List<Article> articles, out int valeursNulles)
        {
            List<Article> articlesTransformes = new List<Article>();
            valeursNulles = 0;

            foreach (var article in articles)
            {
                int nullCount = 0;
                if (article.Id <= 0) nullCount++;
                if (string.IsNullOrEmpty(article.Libelle)) nullCount++;
                if (article.PU <= 0) nullCount++;

                valeursNulles += nullCount;

                if (nullCount == 0)
                {
                    articlesTransformes.Add(article);
                }
            }

            return articlesTransformes.Distinct().ToList();
        }



        private List<Achat> LireTableauAchat(DataTable tableau)
        {
            List<Achat> achats = new List<Achat>();

            foreach (DataRow row in tableau.Rows)
            {
                Achat achat = new Achat
                {
                    Num = Convert.ToInt32(row["Num"]),
                    Id = Convert.ToInt32(row["Id"]),
                    Qte = Convert.ToInt32(row["Qte"]),
                    Date = Convert.ToDateTime(row["Date"])
                };
                achats.Add(achat);
            }

            return achats;
        }

        private List<Vente> LireTableauVente(DataTable tableau)
        {
            List<Vente> ventes = new List<Vente>();

            foreach (DataRow row in tableau.Rows)
            {
                Vente vente = new Vente
                {
                    Num = Convert.ToInt32(row["Num"]),
                    Id = Convert.ToInt32(row["Id"]),
                    Qte = Convert.ToInt32(row["Qte"]),
                    Date = Convert.ToDateTime(row["Date"])
                };
                ventes.Add(vente);
            }

            return ventes;
        }

        private List<Article> LireTableauArticle(DataTable tableau)
        {
            List<Article> articles = new List<Article>();

            foreach (DataRow row in tableau.Rows)
            {
                Article article = new Article
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Libelle = row["Libelle"].ToString(),
                    PU = Convert.ToDecimal(row["PU"])
                };
                articles.Add(article);
            }

            return articles;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            DataTable bilanData = dbHelper.GetBilanData();
            dataGridViewBilan.DataSource = bilanData;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }
    }
}

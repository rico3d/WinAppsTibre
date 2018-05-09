using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures.Model;

namespace WinAppsTibre
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Model model = new Model();

            if (!model.GetConnectionStatus())
            {
                MessageBox.Show(string.Format("Connection failed!", new object[0]));
                Application.Exit();
            }

            var vigas = model.GetModelObjectSelector().GetAllObjectsWithType(ModelObject.ModelObjectEnum.BEAM);



            if (vigas.GetSize() > 0)
            {
                while (vigas.MoveNext())
                {
                    
                    var aPart = vigas.Current as Part;

                    string partMarca = string.Empty;
                    aPart.GetReportProperty("ASSEMBLY_POS", ref partMarca);

                    string partPos = string.Empty;
                    aPart.GetReportProperty("PART_POS", ref partPos);

                    
                    NumberingSeries numberingSeriesItem = 
                        !partPos.Contains('?') ? NumberingSeriesItem(aPart, partPos) : null;

                    NumberingSeries numberingSeriesMarca = 
                        !partMarca.Contains('?') ? NumberingSeriesMarca(partMarca) : null;

                    if (numberingSeriesItem != null)
                    {
                        aPart.PartNumber = numberingSeriesItem;
                    }

                    if (numberingSeriesMarca != null)
                    {
                        aPart.AssemblyNumber = numberingSeriesMarca;
                    }
                   
                   
                    aPart.Modify();



                }



            }
        }

        private NumberingSeries NumberingSeriesItem(Part aPart, string partPos)
        {
            NumberingSeries numberingSeriesItem;
            int compItemAntes = aPart.PartNumber.Prefix.Length;
            string prefixoItem = partPos.Substring(0, compItemAntes);
            int serialItem = int.Parse(partPos.Substring(compItemAntes));
            prefixoItem = textBox2.Text;
            numberingSeriesItem = new NumberingSeries(prefixoItem, serialItem);
            return numberingSeriesItem;
        }

        private NumberingSeries NumberingSeriesMarca(string partMarca)
        {
            NumberingSeries numberingSeriesMarca;
            string prefixoMarca = partMarca.Split('/')[0];
            int serialMarca = int.Parse(partMarca.Split('/')[1]);
            prefixoMarca = txtBoxMDepois.Text;
            numberingSeriesMarca = new NumberingSeries(prefixoMarca, serialMarca);
            return numberingSeriesMarca;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

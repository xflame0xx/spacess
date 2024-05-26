using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sun_planets
{
    public partial class PlanetInfoForm : Form
    {
        private Label lblName;
        private Label lblDiameter;
        private Label lblDistance;
        private Label lblDescription;

        public PlanetInfoForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblName = new Label();
            this.lblDiameter = new Label();
            this.lblDistance = new Label();
            this.lblDescription = new Label();

            this.SuspendLayout();

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new Point(13, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(35, 13);
            this.lblName.Text = "Name:";

            // lblDiameter
            this.lblDiameter.AutoSize = true;
            this.lblDiameter.Location = new Point(13, 40);
            this.lblDiameter.Name = "lblDiameter";
            this.lblDiameter.Size = new Size(52, 13);
            this.lblDiameter.Text = "Diameter:";

            // lblDistance
            this.lblDistance.AutoSize = true;
            this.lblDistance.Location = new Point(13, 67);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new Size(55, 13);
            this.lblDistance.Text = "Distance:";

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new Point(13, 94);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(63, 13);
            this.lblDescription.Text = "Description:";

            // PlanetInfoForm
            this.ClientSize = new Size(284, 261);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblDiameter);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.lblDescription);
            this.Name = "PlanetInfoForm";
            this.Text = "Planet Information";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public void UpdateInfo(string name, string diameter, string distance, string description)
        {
            this.lblName.Text = "Name: " + name;
            this.lblDiameter.Text = "Diameter: " + diameter;
            this.lblDistance.Text = "Distance: " + distance;
            this.lblDescription.Text = "Description: " + description;
        }
    }
}
using Newtonsoft.Json;
using System.Windows.Forms;

namespace javlogexe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Add Button
        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = "titles.json";
            List<Title> titles = new List<Title>();

            // Read and deserialize the existing data
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                titles = JsonConvert.DeserializeObject<List<Title>>(jsonData) ?? new List<Title>();
            }
            else
            {
                titles = new List<Title>();
            }

            if (double.TryParse(textBox2.Text.Trim(), out double newRating))
            {
            }
            else
            {
                MessageBox.Show("The Rating is not a valid number. (0.00 - 100.00)");
            }


            Title newTitle = new Title
            {
                ID = textBox1.Text.Trim(),
                Rating = newRating,
                Comment = textBox3.Text.Trim()
            };

            // Check if the ID already exists
            if (titles.Any(t => t.ID == newTitle.ID))
            {
                MessageBox.Show("ID title already exists.");
            }
            else
            {
                titles.Add(newTitle);
                MessageBox.Show("Title added successfully.");
                // Serialize and save back to JSON
                string updatedJsonData = JsonConvert.SerializeObject(titles, Formatting.Indented);
                File.WriteAllText(filePath, updatedJsonData);
            }
        }

        private List<Title> ReadTitlesFromJson()
        {
            string filePath = "titles.json";

            if (!File.Exists(filePath))
            {
                // Handle the case where the file does not exist
                return new List<Title>();
            }

            try
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Title>>(jsonData) ?? new List<Title>();
            }
            catch (JsonException ex)
            {
                // Handle JSON parsing errors
                // For example, you could log the error or show a message box
                MessageBox.Show("Error reading JSON data: " + ex.Message);
                return new List<Title>();
            }
        }

        //Display Button
        private void button3_Click(object sender, EventArgs e)
        {
            string inputId = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(inputId))
            {
                MessageBox.Show("Please enter an ID to display details.");
                return;
            }

            List<Title> titles = ReadTitlesFromJson();

            Title matchedTitle = titles.FirstOrDefault(t => t.ID.Equals(inputId, StringComparison.OrdinalIgnoreCase));

            if (matchedTitle != null)
            {
                label5.Text = matchedTitle.ID;
                label6.Text = matchedTitle.Rating.ToString();
                label7.Text = matchedTitle.Comment;
            }
            else
            {
                MessageBox.Show("No title found with the given ID.");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        //Remove Button
        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = "titles.json";
            List<Title> titles = new List<Title>();
            string idToDelete = textBox1.Text.Trim();

            // Read and deserialize the existing data
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                titles = JsonConvert.DeserializeObject<List<Title>>(jsonData) ?? new List<Title>();
            }
            else
            {
                titles = new List<Title>();
            }

            // Check if the ID already exists
            if (titles.Any(t => t.ID == idToDelete))
            {
                Title titleToRemove = titles.FirstOrDefault(t => t.ID == idToDelete);
                DialogResult result = MessageBox.Show($"Are you sure you want to delete {idToDelete}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    titles.Remove(titleToRemove);
                    string json = JsonConvert.SerializeObject(titles, Formatting.Indented);
                    File.WriteAllText(filePath, json);
                }
                else if (result == DialogResult.No)
                {

                }
            }
            else
            {
                MessageBox.Show("ID title does not exist.");
            }
        }

        //Modify Button
        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = "titles.json";
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                Console.WriteLine("No titles available to modify.");
                return;
            }

            // Read and deserialize the existing data
            string existingData = File.ReadAllText(filePath);
            List<Title> titles = JsonConvert.DeserializeObject<List<Title>>(existingData) ?? new List<Title>();
            string idToModify = textBox1.Text.Trim();
            Title titleToModify = titles.FirstOrDefault(t => t.ID == idToModify);

            if (titleToModify != null)
            {
                if (double.TryParse(textBox2.Text.Trim(), out double newRating))
                {
                    if (!string.IsNullOrWhiteSpace(textBox2.Text.Trim()))
                    {
                        titleToModify.Rating = Convert.ToDouble(newRating);
                    }
                }
                else
                {
                    MessageBox.Show("The Rating is not a valid number. (0.00 - 100.00)");
                    return;
                }

                string newComment = textBox3.Text.Trim();
                if (!string.IsNullOrWhiteSpace(newComment))
                {
                    titleToModify.Comment = newComment;
                }

                DialogResult result = MessageBox.Show($"Are you sure you want to modify {idToModify}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string json = JsonConvert.SerializeObject(titles, Formatting.Indented);
                    File.WriteAllText(filePath, json);
                    MessageBox.Show("Title modified successfully.");
                }
                else if (result == DialogResult.No)
                {

                }

            }
            else
            {
                MessageBox.Show($"No title found with ID {idToModify}.");
            }
        }

    }
}
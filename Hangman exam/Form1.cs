using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace Hangman_exam
{
    public partial class Form1 : Form
    {
        const string path = "wordList.txt"; //what the word list is called
        string SCORE = "HIGHSCORE.txt"; //what the highscore list is called
        string randomWord = ""; //where the random word is stored
        int TopRowLeft = 430;    //is how far the x value of the row is displaced
        int MiddleRowLeft = 405; //is how far the x value of the row is displaced
        int BottomRowLeft = 350; //is how far the x value of the row is displaced
        int score = 0; //how many rounds the player lasted
        int round = 0; //the round which checks how many lives are left
        bool fail = false;  //if true the game ends
        bool Reset = false; //if true the buttons enable again
        string name = ""; //name stores the players name

        public Form1()
        {
            InitializeComponent();
            this.Height = 400; //set the size of the form
            this.Width = 600;
            TESTLABEL.Visible = false;

            //gets the users name and sets the name variable to the response
            name = Interaction.InputBox("Please enter your name ", "Welcome", "", 10, 10);
            Console.WriteLine(name);
            Console.ReadLine();
            label1.Text = "NAME - " + name;


            List<PictureBox> Hangman = new List<PictureBox>();
            foreach (PictureBox pictureBox in Controls.OfType<PictureBox>())
            {
                if (pictureBox.Tag != null && pictureBox.Tag.ToString() == "Hangman")
                {
                    Hangman.Add(pictureBox);
                }
            }
            foreach (PictureBox pictureBox in Hangman)
            {
                pictureBox.Visible = false;
            } //hides the picture boxes on start

            //top row of keyboard
            List<Button> TopRow = new List<Button>(); //sets the top row of the keybaords position
            foreach (Button button in Controls.OfType<Button>()) //This foreach loop identifies all labels that do not have empty tags and those whose tags are assigned to the keyboard on screen
            {
                if (button.Tag != null && button.Tag.ToString() == "TopRow")
                {
                    TopRow.Add(button);
                }
            }
            foreach (Button button in TopRow)
            {
                button.Top = 225;
                button.Left = TopRowLeft;
                TopRowLeft = TopRowLeft - 35;
            }

            //middle row of keyboard
            List<Button> MiddleRow = new List<Button>(); //sets the middle row of the keybaords position
            foreach (Button button in Controls.OfType<Button>()) //This foreach loop identifies all labels that do not have empty tags and those whose tags are assigned to the keyboard on screen
            {
                if (button.Tag != null && button.Tag.ToString() == "MiddleRow")
                {
                    MiddleRow.Add(button);
                }
            }
            foreach (Button button in MiddleRow)
            {
                button.Top = 265;
                button.Left = MiddleRowLeft;
                MiddleRowLeft = MiddleRowLeft - 35;
            }
            //bottom row of keyboard
            List<Button> BottomRow = new List<Button>(); //sets the bottom row of the keybaords position
            foreach (Button button in Controls.OfType<Button>()) //This foreach loop identifies all labels that do not have empty tags and those whose tags are assigned to the keyboard on screen
            {
                if (button.Tag != null && button.Tag.ToString() == "BottomRow")
                {
                    BottomRow.Add(button);
                }
            }
            foreach (Button button in BottomRow)
            {

                button.Top = 305;
                button.Left = BottomRowLeft;
                BottomRowLeft = BottomRowLeft - 35;
            }

            //Hides all the button
            foreach (Button button in MiddleRow)
            {
                button.Visible = false;
            }
            foreach (Button button in BottomRow)
            {
                button.Visible = false;
            }
            foreach (Button button in TopRow)
            {
                button.Visible = false;
            }
        }


        private void RandomWord(string[] lines)
        {
            Random random = new Random(); //random number generator
            int randomIndex = random.Next(lines.Length); //used to generate a random integer that can be used to select a random word from the list "words"
            randomWord = lines[randomIndex]; // the randomWord selected from the word list using the randomIndex variable
            TESTLABEL.Text = randomWord;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string letter = button.Text;
            letter = letter.ToLower(); //converts the button text to a lowercase
            if (randomWord.Contains(letter) == true)
            {
                int wordlength = 0;
                char check = Convert.ToChar(letter); //Check converts the lowercase letter to a char
                button.BackColor = Color.Green;
                List<Label> PlaceHoldersToChange = new List<Label>(); //this stores all the placeHolders that need to be changed to check if the word letter is correct
                foreach (Label label in Controls.OfType<Label>()) //This foreach loop identifies all labels that do not have empty tags and those whose tags are "placeHolder"
                {
                    if (label.Tag != null && label.Tag.ToString() == "placeHolder")
                    {
                        PlaceHoldersToChange.Add(label);
                    }
                }
                for (int i = 0; i < randomWord.Length; i++) //checks if the letter is in the list and then changes it's exact position
                {
                    if (randomWord[i] == check)
                    {
                        PlaceHoldersToChange[i].Text = letter;
                    }
                }
                for (int i = 0; i < randomWord.Length; i++) //checks if the placeholder doesn't equal what it use to be 
                {
                    if (PlaceHoldersToChange[i].Text != "--")
                    {
                        wordlength++;
                    }
                }
                if (wordlength == randomWord.Length) //if the word length is equal to random word start final sequence
                {
                    string[] lines = File.ReadAllLines(path);
                    RemoveAllPlaceHolders();
                    RandomWord(lines);
                    GenerateLabelsForRandomWord();
                    Reset = true;
                    score++;
                    label2.Text = "Score - " + score;

                    round--;    //makes you go back one incorrect guess if you get it right
                    switch (round)
                    {
                        case 0:
                            picBase.Visible = false;
                            break;

                        case 1:
                            picStand.Visible = false;
                            break;

                        case 2:
                            picSupport.Visible = false;
                            break;

                        case 3:
                            picTop.Visible = false;

                            break;

                        case 4:
                            picHead.Visible = false;
                            break;

                        case 5:
                            picBody.Visible = false;
                            break;

                        case 6:
                            picArms.Visible = false;

                            break;

                        case 7:
                            picLegs.Visible = false;

                            break;
                    }
                }
                button.Enabled = false;
            }
            else if (randomWord.Contains(letter) != true) //disables the button if the letter is not in the word and adds a negative point(image) to the screen
            {
                button.BackColor = Color.Red;
                button.Enabled = false;
                round++;
                switch (round)
                {
                    case 1:
                        picBase.Visible = true;
                        break;

                    case 2:
                        picStand.Visible = true;
                        picBase.Visible = true;

                        break;

                    case 3:
                        picSupport.Visible = true;
                        picStand.Visible = true;
                        picBase.Visible = true;
                        break;

                    case 4:
                        picTop.Visible = true;
                        picStand.Visible = true;
                        picBase.Visible = true;
                        picSupport.Visible = true;

                        break;

                    case 5:
                        picHead.Visible = true;
                        picTop.Visible = true;
                        picStand.Visible = true;
                        picBase.Visible = true;
                        picSupport.Visible = true;

                        break;

                    case 6:
                        picBody.Visible = true;
                        picHead.Visible = true;
                        picTop.Visible = true;
                        picStand.Visible = true;
                        picBase.Visible = true;
                        picSupport.Visible = true;

                        break;

                    case 7:
                        picArms.Visible = true;
                        picBody.Visible = true;
                        picHead.Visible = true;
                        picTop.Visible = true;
                        picStand.Visible = true;
                        picBase.Visible = true;
                        picSupport.Visible = true;
                        break;

                    case 8:
                        fail = true;
                        picArms.Visible = true;
                        picBody.Visible = true;
                        picHead.Visible = true;
                        picTop.Visible = true;
                        picStand.Visible = true;
                        picBase.Visible = true;
                        picSupport.Visible = true;
                        picLegs.Visible = true;
                        fail = true;
                        break;
                }
                if (fail == true) //game end screen checks if the player failed and asks if they want to play again
                {
                    //DialogResult allows the user to select two different options when completeing a challenge
                    DialogResult dr = MessageBox.Show("You lasted the longest with a score of " + score + " Well done your score is saved to an external file, Play again?", "the word was " + randomWord, MessageBoxButtons.YesNo);

                    switch (dr)
                    {
                        case DialogResult.Yes: //if yes the program will reset it's self
                            MessageBox.Show("alright have fun");
                            FileStream AgainFS = new FileStream(SCORE, FileMode.Append);
                            StreamWriter AgainSW = new StreamWriter(AgainFS);
                            AgainSW.WriteLine("PLAYER - " + name + " SCORE - " + score);
                            AgainSW.Close();
                            AgainFS.Close();
                            Application.Restart();
                            break;

                        case DialogResult.No: //if no the program will end it's self
                            MessageBox.Show("goodbye :)");
                            FileStream EndFS = new FileStream(SCORE, FileMode.Append);
                            StreamWriter EndSW = new StreamWriter(EndFS);
                            EndSW.WriteLine("PLAYER - " + name + " SCORE - " + score);
                            EndSW.Close();
                            EndFS.Close();
                            Application.Exit();
                            break;
                    }
                }

            }
            if (Reset == true) //resetes the buttons and makes sure that they are enabled after the correct word is entered
            {
                List<Button> buttonshow = new List<Button>(); //sets the top row of the keybaords position
                foreach (Button Ebutton in Controls.OfType<Button>()) //This foreach loop identifies all labels that do not have empty tags and those whose tags are assigned to the keyboard on screen
                {
                    if (Ebutton.Tag != null && Ebutton.Tag.ToString() == "TopRow")
                    {
                        buttonshow.Add(Ebutton);
                    }
                    if (Ebutton.Tag != null && Ebutton.Tag.ToString() == "MiddleRow")
                    {
                        buttonshow.Add(Ebutton);
                    }
                    if (Ebutton.Tag != null && Ebutton.Tag.ToString() == "BottomRow")
                    {
                        buttonshow.Add(Ebutton);
                    }
                }

                foreach (Button Ebutton in buttonshow)
                {
                    Ebutton.Enabled = true;
                    Ebutton.BackColor = Color.White;
                }
                Reset = false;
            }
        }
        private void RemoveAllPlaceHolders()
        {
            List<Label> placeHoldersToRemove = new List<Label>(); //this stores all the placeHolders that need to be removed before a new word is generated
            foreach (Label label in Controls.OfType<Label>()) //This foreach loop identifies all labels that do not have empty tags and those whose tags are "placeHolder"
            {
                if (label.Tag != null && label.Tag.ToString() == "placeHolder")
                {
                    placeHoldersToRemove.Add(label);
                }
            }
            foreach (Label label in placeHoldersToRemove) //this foreach loop goes through each label in the placeholderstoremove list and removes them
            {
                label.Dispose();
            }
        }
        public void GenerateLabelsForRandomWord()
        {
            Label[] labels = new Label[randomWord.Length]; // Array of Labels of length of random word is declared.
            int xPos = 125; //This is the initial x position of the first placeholder on the form
            for (int i = 0; i < randomWord.Length; i++) //this for loop runs for the number of characters in the random word and creates a new label
            { //setting a bunch of properties including the important tag used to delete them when a new
                labels[i] = new Label(); //word is generated. each label represents a character in the randomword and is initially displayed
                labels[i].Text = "--"; // as two dashes
                labels[i].Location = new Point(xPos, 200);
                labels[i].Enabled = true;
                labels[i].Visible = true;
                labels[i].Width = 30;
                labels[i].Height = 12;
                labels[i].Tag = "placeHolder";
                xPos += 30;
            }
            for (int i = 0; i < randomWord.Length; i++) //this for loop adds each label to the controls collection of the winform
            {
                this.Controls.Add(labels[i]);
            }
        }
        private void btnLaunch_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(path);
            RemoveAllPlaceHolders();
            RandomWord(lines);
            GenerateLabelsForRandomWord();
            btnLaunch.Hide();
            List<Button> buttonshow = new List<Button>(); //sets the top row of the keybaords position
            foreach (Button button in Controls.OfType<Button>()) //This foreach loop identifies all labels that do not have empty tags and those whose tags are assigned to the keyboard on screen
            {
                if (button.Tag != null && button.Tag.ToString() == "TopRow")
                {
                    buttonshow.Add(button);
                }
                if (button.Tag != null && button.Tag.ToString() == "MiddleRow")
                {
                    buttonshow.Add(button);
                }
                if (button.Tag != null && button.Tag.ToString() == "BottomRow")
                {
                    buttonshow.Add(button);
                }
            }

            foreach (Button button in buttonshow) //makes the buttons visable
            {
                button.Visible = true;
            }

        }

        private void picSupport_Click(object sender, EventArgs e)
        {

        }
    }
}

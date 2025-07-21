using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media; // For playing system sounds

namespace CarManagementWinForms
{
    // This class represents a car entity with its attributes and behavior
    public class Car
    {
        // Car properties
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }

        // Constructor to initialize a car object
        public Car(string brand, string model, string color, int year, string type)
        {
            Brand = brand;
            Model = model;
            Color = color;
            Year = year;
            Type = type;
        }

        // This method starts the car and returns a message
        // It's marked virtual to allow overriding in derived classes
        public virtual string Start()
        {
            PlayStartSound(); // Play a sound based on the car type
            return $"{Brand} {Model} started."; // Return confirmation message
        }

        // This method plays a system sound based on the car's type
        public virtual void PlayStartSound()
        {
            switch (Type)
            {
                case "Ordinary":
                    SystemSounds.Beep.Play();         // Basic beep for ordinary cars
                    break;
                case "Race":
                    SystemSounds.Asterisk.Play();     // Asterisk sound for race cars
                    break;
                case "Sports":
                    SystemSounds.Exclamation.Play();  // Exclamation sound for sports cars
                    break;
                case "Super Sports":
                    SystemSounds.Hand.Play();         // Error sound for super sports (more intense)
                    break;
                case "Classic":
                    SystemSounds.Question.Play();     // Question sound for classic cars
                    break;
                default:
                    SystemSounds.Beep.Play();         // Fallback sound
                    break;
            }
        }
    }
}

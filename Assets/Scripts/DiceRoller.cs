using System;
using System.Linq;
using UnityEngine;
using TMPro;

public class DiceRoller : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField; // Reference to TextMeshPro InputField
    [SerializeField] private TMP_Text outputText; // Reference to TextMeshPro UI Text

    void Start()
    {
        // Add listener to detect when user presses Enter in InputField
        inputField.onSubmit.AddListener(HandleInput);
    }

    void OnDestroy()
    {
        // Remove listener when script is destroyed
        inputField.onSubmit.RemoveListener(HandleInput);
    }

    private void HandleInput(string input)
    {
        // Check if the input starts with "/r"
        if (input.StartsWith("/r"))
        {
            try
            {
                // Parse the input and calculate the result
                string result = ParseAndRollDice(input);
                outputText.text = result; // Display the result
            }
            catch (Exception e)
            {
                outputText.text = $"Error: {e.Message}"; // Display error message
            }
        }
        
        inputField.text = ""; // Clear input field
    }

    private string ParseAndRollDice(string input)
    {
        // Example input: "/r 3d6+2" or "/r 1d20-5"
        string command = input.Substring(2).Trim(); // Remove "/r" and trim spaces

        // Regex pattern to match "numberOfDices d maxNumber +/- number"
        var pattern = @"^(\d+)d(\d+)([+-]\d+)?$";
        var match = System.Text.RegularExpressions.Regex.Match(command, pattern);

        if (!match.Success)
        {
            throw new ArgumentException("Invalid command format. Use /r XdY+Z or /r XdY-Z.");
        }

        int numberOfDices = int.Parse(match.Groups[1].Value); // Number of dices
        int maxNumber = int.Parse(match.Groups[2].Value); // Sides of dice
        int modifier = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0; // Modifier (+/-)

        if (numberOfDices <= 0 || maxNumber <= 0)
        {
            throw new ArgumentException("Number of dices and dice sides must be positive.");
        }

        // Roll the dice and calculate the total
        System.Random random = new System.Random();
        int total = 0;
        int[] rolls = new int[numberOfDices];

        for (int i = 0; i < numberOfDices; i++)
        {
            rolls[i] = random.Next(1, maxNumber + 1); // Roll a dice
            total += rolls[i];
        }

        total += modifier; // Apply modifier

        // Format the output
        string rollsText = string.Join(", ", rolls);
        string modifierText = modifier != 0 ? $" {modifier:+#;-#}" : "";
        return $"Rolled {numberOfDices}d{maxNumber}{modifierText}: [{rollsText}] Total = {total}";
    }
}

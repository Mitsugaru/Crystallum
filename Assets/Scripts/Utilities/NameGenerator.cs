using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System.Globalization;
using System.Text;

/// <summary>
/// A variation from http://gamedevelopment.tutsplus.com/tutorials/quick-tip-how-to-code-a-simple-character-name-generator--gamedev-14308
/// </summary>
public class NameGenerator : View, INameGenerator
{

    public int minFirstSyllables = 2;

    public int maxFirstSyllables = 4;

    public int minLastSyllables = 1;

    public int maxLastSyllables = 3;

    public int chanceOfSuffix = 50;

    public bool applySuffixIfNoLastName = false;

    public string[] firstNameSyllables;

    public string[] lastNameSyllables;

    public string[] suffixes;

    protected override void Start()
    {
        base.Start();

        // bounds checking
        if (minFirstSyllables < 0)
        {
            minFirstSyllables = 0;
        }
        if (maxFirstSyllables < minFirstSyllables)
        {
            maxFirstSyllables = minFirstSyllables;
        }
        if (minLastSyllables < 0)
        {
            minLastSyllables = 0;
        }
        if (maxLastSyllables < minLastSyllables)
        {
            maxLastSyllables = minLastSyllables;
        }
        if (chanceOfSuffix < 0)
        {
            chanceOfSuffix = 0;
        }
        else if (chanceOfSuffix > 100)
        {
            chanceOfSuffix = 100;
        }
    }

    public string GenerateName()
    {
        StringBuilder first = new StringBuilder();

        if (firstNameSyllables.Length > 0)
        {
            int firstCount = Random.Range(minFirstSyllables, maxFirstSyllables);
            for (int i = 0; i < firstCount; i++)
            {
                first.Append(firstNameSyllables[Random.Range(0, firstNameSyllables.Length)]);
            }
        }

        if (first.Length > 0)
        {
            first[0] = char.ToUpper(first[0]);
        }

        // Generate last name
        StringBuilder last = new StringBuilder();

        if (lastNameSyllables.Length > 0)
        {
            int lastCount = Random.Range(minLastSyllables, maxLastSyllables);
            for (int i = 0; i < lastCount; i++)
            {
                last.Append(lastNameSyllables[Random.Range(0, lastNameSyllables.Length)]);
            }
        }

        bool applySuffix = Random.Range(0, 100) < chanceOfSuffix;
        // Append suffix to name if applicable
        if (applySuffix && last.Length > 0 && suffixes.Length > 0)
        {
            last.Append(suffixes[Random.Range(0, suffixes.Length)]);
        }
        else if (applySuffix && applySuffixIfNoLastName)
        {
            first.Append(suffixes[Random.Range(0, suffixes.Length)]);
        }

        //Place space between first and last name
        if (last.Length > 0)
        {
            last[0] = char.ToUpper(last[0]);
            if (first.Length > 0)
            {
                last.Insert(0, " ");
            }
        }

        StringBuilder name = new StringBuilder();
        name.Append(first.ToString());
        name.Append(last.ToString());

        return name.ToString();
    }
}

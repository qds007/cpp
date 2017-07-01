#include <cs50.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <ctype.h>

bool search(int value, int values[], int n);

// TASK: Find "needle" number in "haystack" (array of ints): return true if found, false if not. 
int main()
{
	int needle = 1;
	int haystack[7] = { 1, 2, 3, 42, 43, 44, 45 };

	search(needle, haystack, 7);
}


bool search(int value, int values[], int n)
{
	// Return false if n non positive
	if (n <= 0)
	{
		printf("404 needle not found!\n");
		return(false);
	}
	else
	{
		// Get middle value in values
		int midindex = (int)n / 2;
		int midvalue = values[midindex];

		// printf("%i\n", n);
		// printf("%i\n", midindex);
		// printf("%i\n", midvalue);

		// If equal, you found the needle! 
		if (midvalue == value)
		{
			printf("WE FOUND IT!\n");
			return(true);
		}

		// If n = 1 and value is not the target value, then needle not in haystack -> Exit
		else if (n == 1)
		{
			printf("404 needle not found!\n");
			return(false);
		}

		else
		{
			// If midvalue larger, take left half and use this as your new sample space
			// REFERENCED THIS: https://stackoverflow.com/questions/14618342/copying-a-subset-of-an-array-into-another-array-array-slicing-in-c
			if (midvalue > value)
			{
				int newvalues[midindex];

				for (int j = 0; j < midindex; j++)
				{
					newvalues[j] = values[j];
				}

				// Overwrite values, n
				values = newvalues;
				n = midindex;
				search(value, values, n);
			}

			// If midvalue smaller, take right half from midindex + 1 to n
			else
			{
				int newvalues[n - midindex - 1];
				int counter = 0;

				for (int j = midindex + 1; j < n; j++)
				{
					newvalues[counter] = values[j];
					counter++;
				}

				// Overwrite values, n
				values = newvalues;
				n = n - midindex - 1;

				search(value, values, n);
			}
		}
		return(true);
	}
}

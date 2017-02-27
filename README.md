# Next Letter Game Word Finder
CLI to find the best words to use for the Next Letter Game based on the current situation

# Introduction
## Basic Next Letter Gameplay
* Arrange a group of friends/colleagues/coworkers/people.
* The first person says a letter, with a word in mind that that letter spells.
* The next person (the turns rotate in whatever fashion you like) thinks of a word that begins with that fragment and says the next letter. (i.e. if the fragment so far is 'ap' the next letter may be 'p', for 'apple')

## Rules
* If the letter said, added onto the previous fragment (i.e. 'ap' + 'p' = 'app' from the previous example) is known word, that person who said the letter which completed the word is out.

### Example Play with 3 people
* Person 1: 'a' (thinking of 'apposition')
* Person 2: 'p' (to create 'ap', thinking of 'apple')
* Person 3: 'p' (to create 'app', thinking of 'application')
* Person 1 (loop back to beginning): 'l' (to create 'appl', now thinking of 'apple')
* Person 2: 'z' (to create 'applz', bluffing, unable to think of any words that start with 'appl')
* Person 1 challenges, and Person 2, because Person 2 cannot provide a word that starts with 'applz'.
* Person 3 starts the game over with a new letter.

### Challenging
* If someone believes the person who had just provided a letter does _not_ have a word in mind, and is creating a fake word (i.e. someone said 'x' after the fragment is 'ap'), they can challenge.
* Once a challenge has been issued, the challenged body must provide the word they were attempting to spell (i.e. if the person said 'p' after 'ap', they would say 'I was thinking of 'apple' [or 'application' or whatever])
* If no word can be provided (i.e. the player was picking a letter at random), then then the playwe who could not provide their word it out.
* If a word _can_ be provided, the challenger is out.

# NextLetterGame
The NextLetterGame CLI app was created on a whim as an idea to 'beat the house' in the Next Letter Game.

## Use
* When the fragment contains 3 or more letters, launch the program. (2 or less letters made the searching method take longer than desired)
* Input the current fragment, and the program will automatically find the best words to use in the current situation.
* The program will attempt to prune any words which begin with other true words (i.e. if the fragment is 'pol', 'poly' will be returned but 'polygon' will not, beuse in spelling 'polygon' you will spell a real word, 'poly', and be out). For example, if the current fragment is 'app', the program's highest choice will be 'appersonification'.
* To disable automatic pruning, type `~np`. To re-enable it, type `~p`.
* The results will be sorted in terms of length, such that spelling such words will outlive all of the other guesses in the game and potentially recieve a challenge, which you'll win.

## Building
This was built in using C# 6.0 and MSVS 2015. Use comparable or newer versions to compile.

# Notes
This was built in ~20 minutes on a whim. If you find any bugs, please report them or submit a pull request.

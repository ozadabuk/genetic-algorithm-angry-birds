# genetic-algorithm-angry-birds
http://oadabuk.com/code.html#angry
Genetic algorithm discovers an optimal strategy to play a level of Angry Birds and complete the level successfully by eliminating all the pigs.
The goal of this project is to train a computer program to play a level of Angry Birds and complete the level successfully by eliminating all the pigs.
In order to keep the game simple and interesting, I allow he AI to have three yellow birds. I chose the yellow bird because it has a boost activation feature which allows the bird to increase its speed when activated and cause more damage. When it is used in a smart way, yellow bird can cause a lot of damage.
First I built a simple Angry Birds clone in Unity3d, with some physics handling code but that is another tutorial and is in the scope of this project.

Problem Formulation
This is a two step problem and it includes several variables.
These variables are bird’s shooting angle, shooting speed and boost activation time. For a bird to do maximum damage, these parameters have to have an optimal combination. We will accept that each unique combination of these values will represent a unique specimen.
For example a bird that was tossed with 43 degrees angle, with 20m/s speed and activation time 3 seconds is a different bird than that of shot off with 32 degrees angle, 32m/s speed and activation time 1 second.
This is the first step.
Since Angry Birds is a sequential state space (meaning that your previous actions affect the current state space), we have to optimize the combination of birds.
Some birds do more damage when they are combined with certain other birds.
For example, if the first bird destroys the roof, and this bird is combined with the bird that actually lands on top of the head of the last remaining pig, then we have a winning combination. On the other hand if the bird that destroys the roof is combined with the bird that destroys the side wall, and the pig is safe, this is not the best combination. Therefore we aim for the combination of birds that do the most damage.
Genetic Algorithm - GA
Genetic algorithm is inspired by theory of evolution and the laws of genetics. In GA, we start by creating a population. A population is simply a set of states. Each state is called an individual. Just like a real life community of humans or animals.
Some of these individuals are better adapted to their environment than others. To measure how good an individual performs, we assign a performance score. This is how we can distinguish individuals that are doing better.
Performance score can be measured based on the problem’s nature. In our case it would be to based on how much damage inflicted to pigs and their structures. In GA tells that individuals have chromosomes that are made of genes, and these genes give them certain features. Possession of good genes makes individuals perform better, and those who perform better are more likely to pass on their genes to the next generation.
This results in certain good performing genes taking over the population in time. The operation of mating individuals is called cross-over. During the cross-over, and offspring is created and the offspring takes some genes from one parent and other genes from another parent.
Genetic diversity is an important concept in GA. Some genes may perform very well but them may not perform well enough to be a solution (Destroying all of the pigs). If the population lacks genetic diversity, this could lead sub-optimal genes taking over and never reaching a solution. To keep genetic diversity, population size should not be too small.
How to Apply GA to Angry Birds AI?
Create a population of birds. Population with size 50 seems like a good start. The bird chromosome is made of variables shooting angle, shooting speed and boost activation time. Each of these variables represents a gene.
Couple these individual birds to create new generations of better performing, damage yielding birds. I trained about 30 generations of birds for this article.
After training and obtaining a dangerous generation of birds, now we try to find the best 3 bird combinations. Create a population of 3-bird combinations. Start mating these bird combinations.
Resulting bird combinations and bird chromosomes are stunning. Pigs have no chance.
A chromosome for a bird is represented with an array of size 3. [shooting_angle, shooting_speed, boos_activation_time]

A chromosome for 3-bird combination is also represented with an array of size 3. Here the elements of the arrays are indices of trained birds. [bird_1_index, bird_2_index, bird_3_index]

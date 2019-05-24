# dining-philosophers
C# Implementation of the Dining Philosophers

left = Philosopher.LocationAtTable % Program.Forks.Length
right = Philosopher.LocationAtTable % Program.Forks.Length + 1

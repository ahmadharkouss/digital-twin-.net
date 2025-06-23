Le projet est en .NET8.0

# Manufacturing Sort Project

This project is about optimizing the manufacturing order of yogurt pots in a factory. The goal is to minimize the total time taken to produce a batch of yogurt pots with different flavors while adhering to the constraints of the manufacturing line.

[📖 Project Subject](project.md) 

# Simulated Annealing

>A solution corresponds to a state of the system.\
The function f corresponds to the energy of the system.\
The temperature is slowly decreased.\
The lower the temperature, the smaller the chances to increase the energy (increase the energy mean take a solution who didn't had better result).\
The probability to move from state s to s’ with f(s') > f(s) when
the temperature is equal to T is:\
p(s,s’,T) = exp ( f(s) - f(s’) / kB T) )\
where kB is the Boltzmann constant (1.3807 x 10-23).


>Our function take a solution that solution to start, take a random neighbor and check if he had better result, if yes, take the neighbor, if not, check if you still take the neighbor with the function p describe above.

>The temperature and the cooling rate are define before starting the function and depend of the problem, a low cooling rate means that the temperature while decrease slowly, and a high temperature mean that we are accepting badder solutions at start.

# Pseudocode
>s = initial solution (using a heuristic); s* = s = best current solution;\
T = initial temperature; NbIter = NbGel = 0
```Pseudocode
While (T > ε) and (NbIter < MaxIter) and (NbGel < MaxGel)
    OK = false; NbIter = NbIter + 1
    Choose randomly s' ∈ V(s)
    If f(s') < f(s) then OK = true
        else if q < p(s,s',T) (with q uniformly generated in
    [0,1]), then OK = true
    If OK = true then
        If f(s') >= f(s) then NbGel = NbGel + 1, else NbGel = 0
        If f(s') < f(s*) then s* = s'
            s = s'
T =  T
```
Example: MaxIter = 10 000, MaxGel = 1000, &epsilon; = 0.01, &alpha; = 0.995

>We don't use MaxIter or MaxGel in our versions

>More the function goes, less fast the temperature diminish.
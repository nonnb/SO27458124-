## Relating to Stack Overflow answer to `When do you use scope without a statement in C#?`

https://stackoverflow.com/a/27458173/314291

I was proven wrong - scope blocks inside methods have no effect on the GC collection lifespan of objects created within the scope. 

- In debug builds, all variables will be retained until method exit.
- In release builds, any objects proven to have no further access will be eligible for collection at any point in the method



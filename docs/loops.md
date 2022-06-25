# loops

## for loop

```
loop 0 to 10 with "i" {
    println $i;
}
```

by supplying the "i" parameter (which is optional), 
a vartiable with that name is created in the loops execution context.
a loop consists of (moslty) 2 values. if value 1 is less than value 2, 
the loop is ascending (i++), else the loop descends (i--).

whether the loop is ascending or descending, can be calucated by (pseudo code):
```
result = value2 - value1
if result > 0
    the loop is ascending
else if result < 0
    the loop is descending
else 
    the loop repeats 0 times
```
## while(true) loop

spring doesn't implement a regular while loop, but it does provide
a while(true loop). if the expression is omitted, the loop
is interpreted as a while(true) loop

```
loop {
    -- does stuff forever
    --- ...
}
```

## each loop

```
[1, 2, 3, 4, 5, 6] -> $numbers;
each "n" inside $numbers {
    println $n;
} 
```

## breaking out of a loop

to break out of a loop use
```
bail out;
```

## skiping an iteration
to skip an iteration use
```
go next;
```

## general syntax
```
loop expression to expression with "iterator_name" {
    -- ...
}
```

```
loop {
    -- ...
}
```

```
each "element name" inside $collection with "iterator" {
    -- ...
}
```
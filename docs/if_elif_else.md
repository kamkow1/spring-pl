# if statements

# example

```text
True -> $b;

if $b == True {
    println "b is true";
}
else {
    println "b is false";
}
```

# elif
```
5 -> $num;

if $num == 4 {
    println "num is 4";
}
elif $num == 8 {
    println "num is 8";
}
elif $num == 989 {
    println "num is 989";
}
-- and so on
```

# else

```
34 -> $num;

if $num > 0 {
    println "num is greater than 0"; 
}
elif $num < 0 {
    println "num is less than 0";
}
else {
    println "num must be equal to 0";
}
```

# general syntax

```text
if expression {
    -- ...
}
```
# functions

## example function

```
def add_numbers(num1, num2) {
    return num1 + num2;
}
```

## example call

```
add 1, 2 -> $result;

println $result;
```

## returning from a function

you can return with any statement / expression, since they evaluate to null
example

```
def say_hello(name) {
    return "hello " + $name;
}
```

```
def return_with_assignment() {
    return (123 -> $hey);
}
```

```
def returns_null_when_theres_no_return() {
    println "hey";
}

returns_null_when_theres_no_return -> $i_am_null;
```

## general syntax
call a function
```
some_func "arg1", 343, True, -22.2, [1, 2];
```
create a function

```
def func_name (arg1, arg2, arg3) {
    -- do something cool...
}
```
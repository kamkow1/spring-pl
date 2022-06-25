# arrays

## create a empty array

```text
[] -> $arr;
```

## array with values

```text
[1, 2, 3, "hello", "spring"] -> $arr;
```

## add to array

```text
-- ini empty array
[] -> $arr;

arr_add $arr, "hello spring" -> $arr;

-- print with appended value
println $arr; 
```

spring arrays are immutable, so we call a function which edits the array
and returns a new one, so the odl array can be overriden.
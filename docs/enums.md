# enums

example enum
```
[][/]

enum FoodTypes {
    PIZZA,
    PASTA,
    KEBAB
}

def main() {
    println "tell me your favourite meal";

    read_console -> $answer;

    if $answer == FoodTypes.PIZZA {
        println "let's make some pizza";
    }
    elif $answer == FoodTypes.KEBAB {
        println "my fav";
    }
    else {
        println "the healthies option";
    }
}
```

## general syntax 
```
enum Name {
    MEMBER1,
    MEMBER2,
    -- ...
}
```
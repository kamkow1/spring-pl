# includes
## how to include an external file

cat.spring
```
struct Cat {
    pub def meow(t) {
        loop 0 to int $t {
            println "meow!";
        }
    }
}
```

main.spring
```
[]
    include "cat.spring"
[/]

def main(args) {
    -- ...

    new Cat -> $c;

    $c.meow $args[0];
}

```
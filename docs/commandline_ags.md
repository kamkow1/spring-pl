# commandline arguments
## providing args

```text
[][/]

--       notice the args parameter
def main(args) {
    -- do stuff
}
```

to provide args from the commandline use

```bash
spli exec ./main.spring --args 123 --args hello
```

or use the shorter version

```bash

spli exec ./main.spring -a 123 -a hello
```

## accessing args

you access the args like a regular spring array

```text
[][/]

def main(args) {
    each "argument" inside $args with "i" {
        println "arg no. " + string i;
        println $argument;
    }
}
```
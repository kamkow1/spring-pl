
# Spring Programming Language

Spring is an interpreted object-oriented programming language 
with it's syntax resambling Python, C and PHP.



## Features

- structs
- encapsulation
- functions
- variables
- loops
- if blocks
- elif and else blocks
- each (foreach) loops
- arrays


## Usage/Examples

```text
[]
[/]

struct Person {
    prv prop mood;

    pub def setMood(self, v) {
        $v -> $self.mood;
    }

    pub def sayMood(self) {
        if $self.mood == "happy" {
            println "yay im happy!";
        }
        elif $self.mood == "sad" {
            println "im sad ;(";
        }
        else {
            println "unknown mood...";
        }
    }
}

-- use commandline arguments
def main(args) {
    new Person -> $p;

    $p.setMood $p, string $args[0];

    $p.sayMood $p;
}
```


## Documentation
coming soon


## building from source

ignore all the warnings. they are caused by antlr4's c# target.
```bash
git clone https://github.com/kamkow1/spring-pl.git
cd spring-pl
cd src/spli
./scripts/build.sh
```
## download
all binaries are located in the publish directory at the root of this repo.
the binaries are self-contained, therefore don't require dotnet to run (dotnet is included in the binary)

## to add
a list o features to add to the language
 - json serialization / deserialization
 - xml parsing
 - lambda expressions
 - enums
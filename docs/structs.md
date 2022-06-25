# structs and oop in spring

## create a struct

```
struct MyVeryCoolStruct {
    -- ...
}
```
## add properties

```
struct MyVeryCoolStruct {
    pub prop i_am_public;

    prv prop i_am_private;
}
```

## add methods

```
struct MyVeryCoolStruct {
    pub prop i_am_public;

    prv prop i_am_private;

    def set_i_am_private(self, v) {
        $v -> $self.i_am_private;
    }

    def get_i_am_private(self) {
        return $self.i_am_private;
    }
}
```

## instantiate a struct

```
struct MyVeryCoolStruct {
    pub prop i_am_public;

    prv prop i_am_private;

    def set_i_am_private(self, v) {
        $v -> $self.i_am_private;
    }

    def get_i_am_private(self) {
        return $self.i_am_private;
    }
}

def main() {
    new MyVerCoolStruct -> $mvcs;

    $mvcs.set_i_am_private $mvcs, "hey";

    println $mvcs.get_i_am_private $mvcs;
}
```
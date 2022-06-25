namespace spli.Interpreter;

public class Structure
{
    public Dictionary<string, Method> Methods { get; set; }

    public Dictionary<string, Prop> Props { get; set; }

    public Structure(Dictionary<string, Method> methods, 
                    Dictionary<string, Prop> props)
    {
        Methods = methods;
        Props = props;
    }
}
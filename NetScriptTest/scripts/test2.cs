public partial class Core
{
    public string GetTestPartial()
    {
        return "Partial #";
    }
}
public class test2
{
    public test2(Globals global)
    {
        global.Character.Debug("init test 2");
    }
}
public partial class Core
{
    public Globals global = new Globals();
    test2 t2;
    int i = 0;
    public void initialize()
    {
        i++;
        t2 = new test2(global);
        test1.test1v(global);
        global.Character.Debug(GetTestPartial() + global.Character.ID.ToString());
    }
    public int GetCallCount()
    {
        return i;
    }
}
public class test1
{
    public static void test1v(Globals global)
    {
        global.Character.Debug("test static method");
    }
}
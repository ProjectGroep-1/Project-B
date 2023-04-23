public class TableLogic
{
    public List<Table> _tableList;

    public TableLogic()
    {
        _tableList = TableAccess.LoadTableJSON();
    }
}
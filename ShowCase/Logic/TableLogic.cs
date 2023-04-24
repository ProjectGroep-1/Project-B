public class TableLogic
{
    public List<Table> _tableList;

    public TableLogic()
    {
        _tableList = TableAccess.LoadTableJSON();
    }

    public void UpdateList(Table table)
    {
        //Find if there is already an model with the same id
        int index = _tableList.FindIndex(s => s.Id == table.Id);

        if (index != -1)
        {
            //update existing model
            _tableList[index] = table;
        }
        else
        {
            //add new model
            _tableList.Add(table);
        }
        TableAccess.WriteAll(_tableList);

    }
}
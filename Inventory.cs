namespace InventoryAPI
{
    public class Inventory
    {
        //This is the primary key value for the Inventory table
        public int InventoryID { get; set; }

        public string APIID { get; set; } = string.Empty;

        public int SerialNumber { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Number { get; set; }

        public int StatusID { get; set; }
        //This is the foreign key value for the Operator table in the Poeple database.
        public int OperatorID { get; set; }
        //This is a foreign key to the Tract table in the same database.
        public int TractID { get; set; }
        //This is the foreign key value to the InventoryType table in the same database.
        public int InventoryTypeID { get; set; }
    }
}
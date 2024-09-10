using SqlSugar;

namespace Modules.CRM.Domain.DbEntity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Test")]
    public partial class TestEntity
    {
        public TestEntity()
        {


        }

        [SugarColumn(IsPrimaryKey = true, Length = 36)]
        public string ID { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string ITEM_TYPE_ID { get; set; }


        [SugarColumn(Length = 128, IsNullable = true)]
        public string KEYED_NAME { get; set; }


        [SugarColumn(Length = 11, IsNullable = true)]
        public DateTime? CREATED_ON { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string CREATED_BY_ID { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string CREATED_BY_ID_ITI { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string OWNED_BY_ID { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string OWNED_BY_ID_ITI { get; set; }


        [SugarColumn(Length = 11)]
        public DateTime MODIFIED_ON { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string MODIFIED_BY_ID { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string MODIFIED_BY_ID_ITI { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string STATE { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string STATE_ITI { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string LOCKED_BY_ID { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string LOCKED_BY_ID_ITI { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool IS_CURRENT { get; set; }


        [SugarColumn(Length = 8, IsNullable = true)]
        public string MAJOR_REV { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string MANAGED_BY_ID { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string MANAGED_BY_ID_ITI { get; set; }


        [SugarColumn(Length = 8, IsNullable = true)]
        public string MINOR_REV { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool IS_RELEASED { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool NOT_LOCKABLE { get; set; }


        [SugarColumn(Length = 10, IsNullable = true)]
        public long? GENERATION { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool NEW_VERSION { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string CONFIG_ID { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string CONFIG_ID_ITI { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string PERMISSION { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string PERMISSION_ITI { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string LAYER { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string LAYER_ITI { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string VERSIONGROUP { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool ENABLED { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string ICONRESOURCE { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string ICONRESOURCE_ITI { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string SOURCE_ID { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string SOURCE_ID_ITI { get; set; }


        [SugarColumn(Length = 22, IsNullable = true)]
        public double? SUMQUANTITY { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool ACQUISITIONSTATE { get; set; }


        [SugarColumn(Length = 512, IsNullable = true)]
        public string CODESUM { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool WORKROUT { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string MATERIALDEFINITION { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string MATERIALDEFINITION_ITI { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string MAINPLANT { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string MAINPLANT_ITI { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string ROUTE { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string PLANNUMBER { get; set; }


        [SugarColumn(Length = 1280, IsNullable = true)]
        public string WORKDEFINITIONID { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string NOTE { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool LOCK { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string SUBCONTRACTING { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string SUBCONTRACTING_ITI { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string INQUANTITY { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string INWORKSHOP { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool METALPROCESSING { get; set; }


        [SugarColumn(Length = 10, IsNullable = true)]
        public long? REPULSECOOPERATION { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? COSTOFSIMULATION { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? ACTUALCOST { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? PROCESSCOST { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string BUFFERSTATE { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string BUFFERSTATE_ITI { get; set; }


        [SugarColumn(Length = 1, IsNullable = true)]
        public bool MREQ { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? QTY_REQ { get; set; }


        [SugarColumn(Length = 200, IsNullable = true)]
        public string DATE_REQ { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string PRECONCERTEDLINE { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string MREQ_ORDER { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? FINISHED { get; set; }


        [SugarColumn(Length = 200, IsNullable = true)]
        public string SUB_DELIVERYDATE { get; set; }


        [SugarColumn(Length = 1024, IsNullable = true)]
        public string ROUTENOTE { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string ISKEYPART { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string ITPPLAN { get; set; }


        [SugarColumn(Length = 11, IsNullable = true)]
        public DateTime? ROUGHCASTOUTGOINGTIME { get; set; }


        [SugarColumn(Length = 11, IsNullable = true)]
        public DateTime? PRODUCTOUTGOINGTIME { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string TOPUSEINFORMATION { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string ROUGHCASTINCOMING { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string ROUGHCASTIDENTIFICATION { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string PRODUCTIDENTIFICATION { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string PARTIDENTIFICATION { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? ROUGHCASTQUANTITY { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string SHORTROUTE { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string DEMARCATE { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string PLANSTA { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string PLANSTA_ITI { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string ORG { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string ORG_ITI { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string BLANKREMARK { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string BLANKLOCATION { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string FINISHEDGOODREMARK { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string FINISHEDGOODLOCATION { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? FINISHED_QUANTITY { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string HEATNUMBER { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string ISCOOPER { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string ISCOOPER_ITI { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string COOPERNOTE { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? COOPERNUM { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string PLANORG { get; set; }


        [SugarColumn(Length = 36, IsNullable = true)]
        public string PLANORG_ITI { get; set; }


        [SugarColumn(Length = 11, IsNullable = true)]
        public DateTime? ROUGHCASTINTIME { get; set; }


        [SugarColumn(Length = 11, IsNullable = true)]
        public DateTime? FINISHED_INPUTDATE { get; set; }


        [SugarColumn(Length = 11, IsNullable = true)]
        public DateTime? FINISHEDTIME { get; set; }


        [SugarColumn(Length = 10, IsNullable = true)]
        public long? ELECPROCESS { get; set; }


        [SugarColumn(Length = 10, IsNullable = true)]
        public long? ELECIMAGE { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string GETID { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string SKILLREQUIREMENT { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string SPECIFICATION { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string TEXTURE { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string DESCRIPTION { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string ITEMNUMBER { get; set; }


        [SugarColumn(Length = 18, IsNullable = true)]
        public decimal? PIECEWEIGHT { get; set; }


        [SugarColumn(Length = 256, IsNullable = true)]
        public string UNITTEXT { get; set; }


        [SugarColumn(Length = 11, IsNullable = true)]
        public DateTime? PURCHASEDINTIME { get; set; }

    }
}

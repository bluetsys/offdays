using System.Xml.Serialization;

// 요청변수
var _SolYear = 2023;    // 년
var _SolMonth = 10;     // 월

var _Path = System.IO.Path.Combine("Data");

// 폴더 생성
System.IO.Directory.CreateDirectory(_Path);

// 파일 이름 제작
_Path = System.IO.Path.Combine(_Path, $"{_SolYear.ToString("0000")}-{_SolMonth.ToString("00")}.xml");

// 파일 존재 유무
if( !System.IO.File.Exists(_Path) )
{

    // 요청 변수 만들기
    var _QueryString = System.Web.HttpUtility.ParseQueryString("");

	_QueryString.Add("solYear", _SolYear.ToString("0000")); 			             // 년 solYear
	_QueryString.Add("solMonth", _SolMonth.ToString("00"));          				 // 월 solMonth
	_QueryString.Add("serviceKey",  "서비스키");     // 공공데이터포털에서 받은 인증키

    System.Net.WebClient _WebClient = new System.Net.WebClient();

    // 데이터 요청 인코딩 설정
    _WebClient.Encoding = System.Text.Encoding.UTF8;

    // 파일로 저장
    _WebClient.DownloadFile($"https://apis.data.go.kr/B090041/openapi/service/SpcdeInfoService/getHoliDeInfo?{_QueryString.ToString()}", _Path);
}

// 파일 읽기
var _Body = System.IO.File.ReadAllText(_Path);

// 직렬화 선언
var _Serializer = new System.Xml.Serialization.XmlSerializer(typeof(Response));

// 직렬화
using (StringReader reader = new StringReader(_Body))
{
   var test = (Response)_Serializer.Deserialize(reader);

    System.Console.WriteLine($"ResultCode : {test.Header.ResultCode}");
    System.Console.WriteLine($"ResultMsg : {test.Header.ResultMsg}");

    System.Console.WriteLine($"TotalCount : {test.Body.TotalCount}");
    System.Console.WriteLine($"NumOfRows : {test.Body.NumOfRows}");
    System.Console.WriteLine($"PageNo : {test.Body.PageNo}");

    foreach(var item in test.Body.Items.Item)
    {
        System.Console.WriteLine($"\t============================");
        System.Console.WriteLine($"\tDateKind : {item.DateKind}");
        System.Console.WriteLine($"\tDateName : {item.DateName}");
        System.Console.WriteLine($"\tIsHoliday : {item.IsHoliday}");
        System.Console.WriteLine($"\tLocdate : {item.Locdate}");
        System.Console.WriteLine($"\tSeq : {item.Seq}");
    }
}

[XmlRoot(ElementName="header")]
public class Header { 

	[XmlElement(ElementName="resultCode")] 
	public int ResultCode { get; set; } 

	[XmlElement(ElementName="resultMsg")] 
	public string ResultMsg { get; set; } 
}

[XmlRoot(ElementName="item")]
public class Item { 

	[XmlElement(ElementName="dateKind")] 
	public int DateKind { get; set; } 

	[XmlElement(ElementName="dateName")] 
	public string DateName { get; set; } 

	[XmlElement(ElementName="isHoliday")] 
	public string IsHoliday { get; set; } 

	[XmlElement(ElementName="locdate")] 
	public int Locdate { get; set; } 

	[XmlElement(ElementName="seq")] 
	public int Seq { get; set; } 
}

[XmlRoot(ElementName="items")]
public class Items { 

	[XmlElement(ElementName="item")] 
	public List<Item> Item { get; set; } 
}

[XmlRoot(ElementName="body")]
public class Body { 

	[XmlElement(ElementName="items")] 
	public Items Items { get; set; } 

	[XmlElement(ElementName="numOfRows")] 
	public int NumOfRows { get; set; } 

	[XmlElement(ElementName="pageNo")] 
	public int PageNo { get; set; } 

	[XmlElement(ElementName="totalCount")] 
	public int TotalCount { get; set; } 
}

[XmlRoot(ElementName="response")]
public class Response { 

	[XmlElement(ElementName="header")] 
	public Header Header { get; set; } 

	[XmlElement(ElementName="body")] 
	public Body Body { get; set; } 
}

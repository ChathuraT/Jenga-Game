[System.Serializable]
public class JengaBlockData
{
    public int id;
    public string subject;
    public string grade;
    public int mastery;
    public string domainid;
    public string domain;
    public string cluster;
    public string standardid;
    public string standarddescription;

    /// <summary>
    /// Initializes a new instance of the JengaBlockData class with the specified data.
    /// </summary>
    public JengaBlockData(int id, string subject, string grade, int mastery, string domainid, string domain, string cluster, string standardid, string standarddescription)
    {
        this.id = id;
        this.subject = subject;
        this.grade = grade;
        this.mastery = mastery;
        this.domainid = domainid;
        this.domain = domain;
        this.cluster = cluster;
        this.standardid = standardid;
        this.standarddescription = standarddescription;
    }
}

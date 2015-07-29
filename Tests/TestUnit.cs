using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace TestMVC4App.Models
{
    public abstract class TestUnit
    {
        protected Dictionary<string, string> suffixMapping = new Dictionary<string, string>() { 
            {"A.A.","AA"},
            {"Associates of Arts","AA"},
            {"ABPP","ABPP"},
            {"ABPP-CN","ABPP"},
            {"ACHPN","ACHPN"},
            {"ACNP-BC","ACNP (BC)"},
            {"ACSM-HFS","ACSM-HFS"},
            {"ADS","ADS"},
            {"AGAF","AGAF"},
            {"ANP-BC","ANP (BC)"},
            {"APRN","APRN"},
            {"APRN BC","APRN (BC)"},
            {"APRN-BC","APRN (BC)"},
            {"ARDMS","ARDMS"},
            {"Associate Degree","AS"},
            {"Associates Degree","AS"},
            {"ATR","ATR"},
            {"Au.D.","AuD"},
            {"AUD","AuD"},
            {"B.A.","BA"},
            {"B.A. Geography","BA"},
            {"B.A. Molecular","BA"},
            {"BA","BA"},
            {"BA 1951","BA"},
            {"BA Accounting St. Leo University 1983","BA"},
            {"BA Biology","BA"},
            {"BA psychology","BA"},
            {"Bachelor of Arts","BA"},
            {"Bachelor of Arts in English","BA"},
            {"Psychology B.A.","BA"},
            {"BAO","BAO"},
            {"BCBA-D","BCBA-D"},
            {"BCh","BCh"},
            {"ChB","BCh"},
            {"BCPS","BCPS"},
            {"B.Ed.","BEd"},
            {"BEngSci","BEngSci"},
            {"B.F.A.","BFA"},
            {"BFA - Graphic Design","BFA"},
            {"BMBS BMedSci","BMBS"},
            {"BME","BME"},
            {"BMedSc","BMedSci"},
            {"B. Phil","BPhil"},
            {"B.S","BS"},
            {"B.S.","BS"},
            {"B.S. Biology","BS"},
            {"B.S. Molecular Biochemistry and Biophysics","BS"},
            {"B.S. Psychology","BS"},
            {"B.Sc. Computer Science","BS"},
            {"Bachelor of Science","BS"},
            {"Bachelor of Science - Microbiology (2009)","BS"},
            {"Bachelor of Science Pathobiology","BS"},
            {"Bachelor of Science Psychology","BS"},
            {"BPsych","BS"},
            {"BS","BS"},
            {"BS Biochemistry - University of Wisconsin","BS"},
            {"BS recreation and leisure studies","BS"},
            {"BSc","BS"},
            {"BSc.(Hons)","BSc (Hon)"},
            {"BSN","BSN"},
            {"BTech","BTech"},
            {"BVSc","BVSc"},
            {"C-TAGME","C-TAGME"},
            {"CCC-A","CCC (A)"},
            {"CCC-SLP","CCC (SLP)"},
            {"CCC/SLP","CCC (SLP)"},
            {"CCRC","CCRC"},
            {"CCRP","CCRP"},
            {"CDE","CDE"},
            {"C.G.C.","CGC"},
            {"CHES","CHES"},
            {"CIP","CIP"},
            {"CMD","CMD"},
            {"CNE","CNE"},
            {"CNM","CNM"},
            {"CNS","CNS"},
            {"CNS-BC","CNS (BC)"},
            {"CPC","CPC"},
            {"CPCS","CPCS"},
            {"CPE","CPE"},
            {"CPH","CPH"},
            {"CPM","CPM"},
            {"CPNP","CPNP"},
            {"CRNA","CRNA"},
            {"DABR","DABR"},
            {"DACLAM","DACLAM"},
            {"diplomate ACLAM","DACLAM"},
            {"DACVP","DACVP"},
            {"DClinPsy","DClinPsych"},
            {"DDS","DDS"},
            {"DFAAPA","DFAAPA"},
            {"DFAPA","DFAPA"},
            {"DHL (Hon.)","DHL (Hon)"},
            {"Dip.Ed","DipEd"},
            {"DLFAPA","DLFAPA"},
            {"DMD","DMD"},
            {"DMedSci","DMedSci"},
            {"DML","DML"},
            {"DNB","DNB"},
            {"DNSc","DNSc"},
            {"DO","DO"},
            {"D.Phil.","DPhil"},
            {"DPhil","DPhil"},
            {"DPM","DPM"},
            {"Dr. Med.","Dr Med"},
            {"Dr. Med. (h.c.)","Dr Med"},
            {"DrPH","DrPH"},
            {"D.Sc.","DSc"},
            {"DSc","DSc"},
            {"DTMH","DTMH"},
            {"DVM","DVM"},
            {"DEd","EdD"},
            {"Ed.D","EdD"},
            {"Ed.D.","EdD"},
            {"EdD","EdD"},
            {"Esq","Esq"},
            {"Esq.","Esq"},
            {"FAAA","FAAA"},
            {"FAAD","FAAD"},
            {"FAAEM","FAAEM"},
            {"FAAMA","FAAMA"},
            {"FAAN","FAAN"},
            {"FAANS","FAANS"},
            {"FAAOS","FAAOS"},
            {"FAAP","FAAP"},
            {"FAAPM","FAAPM"},
            {"F.A.C.C.","FACC"},
            {"FACC","FACC"},
            {"FACE","FACE"},
            {"FACEP","FACEP"},
            {"FACFAS","FACFAS"},
            {"FACG","FACG"},
            {"FACM","FACM"},
            {"FACMI","FACMI"},
            {"FACMT","FACMT"},
            {"FACNM","FACNM"},
            {"FACNS","FACNS"},
            {"F.A.C.O.G.","FACOG"},
            {"FACOG","FACOG"},
            {"Fellow American College of Obstetrics and Gynecology","FACOG"},
            {"FACP","FACP"},
            {"FACPM","FACPM"},
            {"FACR","FACR"},
            {"F.A.C.S.","FACS"},
            {"FACS","FACS"},
            {"FAHA","FAHA"},
            {"FAPA","FAPA"},
            {"FASCRS","FASCRS"},
            {"FASGE","FASGE"},
            {"FASH","FASH"},
            {"FASN","FASN"},
            {"FASTRO","FASTRO"},
            {"FAWM","FAWM"},
            {"FBA","FBA"},
            {"FCCM","FCCM"},
            {"FCCP","FCCP"},
            {"FCS (SA)","FCS (SA)"},
            {"FESC","FESC"},
            {"FFARCSI","FFARCSI"},
            {"MD FFARCSI","FFARCSI"},
            {"FISMRM","FISMRM"},
            {"FNP","FNP"},
            {"FNP-BC","FNP (BC)"},
            {"FPMHNP","FPMHNP"},
            {"FRACP","FRACP"},
            {"F.R.C.P.","FRCP"},
            {"FRCP","FRCP"},
            {"FRCP(C)","FRCP(C)"},
            {"FRCPC","FRCP(C)"},
            {"FRCPsych","FRCPsych"},
            {"FRCS","FRCS"},
            {"FRCS (Eng & Ed)","FRCS (Eng & Ed)"},
            {"FRCS (C)","FRCS(C)"},
            {"FRCS(C)","FRCS(C)"},
            {"FRS","FRS"},
            {"FRCS(Ed)","FRSC (Ed)"},
            {"F.S.C.A.I.","FSCAI"},
            {"FACC FSCAI","FSCAI"},
            {"FSCBT/MR","FSCBT/MR"},
            {"HCLD","HCLD"},
            {"HT(ASCP)","HT (ASCP)"},
            {"II","II"},
            {"III","III"},
            {"ITILv3","ITIL v3"},
            {"IV","IV"},
            {"J.D.","JD"},
            {"JD","JD"},
            {"Jr","Jr"},
            {"Jr.","Jr"},
            {"LADC","LADC"},
            {"LATg","LATg"},
            {"L.C.S.W.","LCSW"},
            {"LCSW","LCSW"},
            {"LL.B.","LLB"},
            {"LLM","LLM"},
            {"LMSW","LMSW"},
            {"LPC","LPC"},
            {"Licensed Practical Nurse","LPN"},
            {"M.A","MA"},
            {"M.A.","MA"},
            {"M.A. Food Geography","MA"},
            {"M.A. of clinical psychology","MA"},
            {"MA","MA"},
            {"MA 1954","MA"},
            {"MA Southern Connecticut State University","MA"},
            {"MAE","MAE"},
            {"M.A.Ed.","MAEd"},
            {"M.A.L.S.","MALS"},
            {"M.A.R.","MAR"},
            {"MAT","MAT"},
            {"M.B.","MB"},
            {"MB","MB"},
            {"M.B.A.","MBA"},
            {"MBA","MBA"},
            {"MBA (Candidate 2014)","MBA"},
            {"MB BChir","MBBCh"},
            {"MB ChB","MBBCh"},
            {"MBBCh","MBBCh"},
            {"MBChB","MBBCh"},
            {"M.B.B.S.","MBBS"},
            {"MBBS","MBBS"},
            {"MBBS (MD)","MBBS"},
            {"MBE","MBE"},
            {"MBioMedSci","MBioMedSci"},
            {"MChir","MChir"},
            {"MCIS","MCIS"},
            {"DM.","MD"},
            {"Jr. MD","MD"},
            {"M.D","MD"},
            {"M.D.","MD"},
            {"MD","MD"},
            {"MD MBA","MD"},
            {"MD MHS","MD"},
            {"MD MPH","MD"},
            {"MD MS","MD"},
            {"MD MSc","MD"},
            {"MD PhD MRCP(UK)","MD"},
            {"MD.","MD"},
            {"Medical Doctor","MD"},
            {"MD PhD","MD/PhD"},
            {"MD-PhD","MD/PhD"},
            {"MD; PhD","MD/PhD"},
            {"MD/PhD","MD/PhD"},
            {"MDm Ph.D","MD/PhD"},
            {"MDCM","MDCM"},
            {"MDiv","MDiv"},
            {"EdM","MEd"},
            {"M.Ed","MEd"},
            {"M.Ed.","MEd"},
            {"MEd","MEd"},
            {"MEM","MEM"},
            {"MFA","MFA"},
            {"MFT","MFT"},
            {"MHA","MHA"},
            {"MHCM","MHCM"},
            {"MHE","MHE"},
            {"M.H.S.","MHS"},
            {"MHS","MHS"},
            {"MHSA","MHSA"},
            {"MLS","MLS"},
            {"MMHC","MMHC"},
            {"MMM","MMM"},
            {"MMS","MMSc"},
            {"MMSc","MMSc"},
            {"MMSc.","MMSc"},
            {"MPA","MPA"},
            {"MPAS","MPAS"},
            {"MPE","MPE"},
            {"MPH CHES","MPH"},
            {"M.P.H","MPH"},
            {"M.P.H.","MPH"},
            {"Master of Public Health","MPH"},
            {"MPH","MPH"},
            {"MPH University of California","MPH"},
            {"M.Phil","MPhil"},
            {"Master of Philosophy","MPhil"},
            {"MPhil","MPhil"},
            {"MPP","MPP"},
            {"MPS","MPS"},
            {"MRCOG (UK)","MRCOG"},
            {"MRCP","MRCP"},
            {"MRCP(UK)","MRCP"},
            {"MRCPSYCH","MRCPsych"},
            {"MRes","MRes"},
            {"M.S","MS"},
            {"M.S.","MS"},
            {"M.S. (Med)","MS"},
            {"M.S. APRN","MS"},
            {"M.S. Comp Sci","MS"},
            {"M.S. in Public Service Management","MS"},
            {"M.Sc.","MS"},
            {"M.Sci.","MS"},
            {"Master of Science","MS"},
            {"MS","MS"},
            {"MS Applied Genomics","MS"},
            {"MS in Education","MS"},
            {"MS MSPH FACEP","MS"},
            {"MSc","MS"},
            {"MSc. (Community Health and Epidemiology)","MS"},
            {"MSCI","MS"},
            {"MSCS","MS"},
            {"MSEd","MS"},
            {"MSHI","MS"},
            {"MSHS","MS"},
            {"Sc. M.","MS"},
            {"ScM","MS"},
            {"SM","MS"},
            {"MSCE","MSCE"},
            {"MSCR","MSCR"},
            {"M.S.E.L.","MSEL"},
            {"MSM","MSM"},
            {"MSN","MSN"},
            {"MSOM","MSOM"},
            {"MSSA","MSSA"},
            {"MSSW","MSSW"},
            {"MStJ","MStJ"},
            {"M.S.W.","MSW"},
            {"Master in Social Work","MSW"},
            {"MSW","MSW"},
            {"MT","MT"},
            {"MTR","MTR"},
            {"MTS","MTS"},
            {"ND","ND"},
            {"NP","NP"},
            {"NP-C","NP-C"},
            {"OCN","OCN"},
            {"OD","OD"},
            {"PA","PA"},
            {"PA-C","PA-C"},
            {"PA(ASCP)","PA(ASCP)"},
            {"PCCN","PCCN"},
            {"PharmD","PharmD"},
            {"Ph D","PhD"},
            {"Ph. D","PhD"},
            {"Ph.D","PhD"},
            {"Ph.D.","PhD"},
            {"Ph.D. 1987","PhD"},
            {"Ph.D. Clinical Psychology","PhD"},
            {"PhD","PhD"},
            {"PhD MA","PhD"},
            {"PhD (Medical Oncology)","PhD"},
            {"PhD 1959","PhD"},
            {"PhD Clin","PhD"},
            {"PhD Clinical Psychology","PhD"},
            {"PhD Cultural Anthropology","PhD"},
            {"PhD Fellow","PhD"},
            {"PhD MAppStat","PhD"},
            {"PhM","PhM"},
            {"PMHNP-BC","PMHNP (BC)"},
            {"PMP","PMP"},
            {"PNP-BC","PNP (BC)"},
            {"PSM","PSM"},
            {"Psy.D.","PsyD"},
            {"PsyD","PsyD"},
            {"RD","RD"},
            {"RDCS","RDCS"},
            {"RDMS","RDMS"},
            {"RM","RM"},
            {"RMT","RMT"},
            {"RN","RN"},
            {"RNP","RNP"},
            {"RPSGT","RPsgT"},
            {"RPVI","RPVI"},
            {"RRT","RRT"},
            {"ScD","ScD"},
            {"Sr","Sr"},
            {"Sr.","Sr"},
            {"V","V"},
            {"VI","VI"},
            {"WHNP-BC","WHNP (BC)"}
        };

        # region Properties

        public bool HttpErrorHappened { get; set; }

        public bool UnknownErrorHappened { get; set; }

        public string ErrorMessage { get; set; }

        public virtual string NewServiceURLExtensionBeginning
        {
            get { return "Users/PageName/"; }
        }

        public virtual string NewServiceURLExtensionEnding
        {
            get { return "/Complete"; }
        }

        public TestSuite Container { get; set; }

        public TestUnit Parent { get; set; }

        public HashSet<TestUnit> Children { get; set; }

        public Dictionary<EnumTestUnitNames,ResultReport> DetailedResults { get; set; }

        public EnumResultSeverityType OverallSeverity { get; private set; }

        protected int OldId { get; private set; }

        public int UserId { get; protected set; }

        protected string PageName { get; private set; }

        protected IEnumerable<XElement> OldDataNodes { get; private set; }

        #endregion Properties

        /// <summary>
        /// Defines the overall Result Severity by counting the occurences of Severity Types in the detailed Results.
        /// </summary>
        public void ComputeOverallSeverity()
        {
            bool keepGoing = true;

            var errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.ERROR).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if(keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.ERROR;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.ERROR_WITH_EXPLANATION).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.ERROR_WITH_EXPLANATION;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.FALSE_POSITIVE).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.FALSE_POSITIVE;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.WARNING).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.WARNING;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.SUCCESS).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.SUCCESS;
                keepGoing = false;
            }
        }

        /// <summary>
        /// Adds the children of children to this level so that results can be computed together.
        /// </summary>
        public void ComputerOverallResults()
        {
            foreach (var child in Children)
            {
                Array.ForEach(child.DetailedResults.ToArray(), x => this.DetailedResults.Add(x.Key,x.Value));
            }
        }

        /// <summary>
        /// Creates the full path to the new web service data for this specific test.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected virtual string BuildNewServiceURL(string pageName)
        {
            if (this.Container == null)
            {
                throw new NotImplementedException();
            }

            return Container.newServiceURLBase + NewServiceURLExtensionBeginning + pageName + NewServiceURLExtensionEnding;
        }

        protected TestUnit(TestSuite container, TestUnit parent = null)
        {
            this.Container = container;
            this.Parent = parent;
            this.Children = new HashSet<TestUnit>();
            this.DetailedResults = new Dictionary<EnumTestUnitNames,ResultReport>();
        }

        protected abstract void RunAllSingleTests();

        public void ProvideData(int OldId, IEnumerable<XElement> oldDataNodes, int userId, string pageName)
        {
            this.OldId = OldId;
            this.OldDataNodes = oldDataNodes;
            this.UserId = userId;
            this.PageName = pageName;
        }

        public void RunAllTests()
        {
            try
            {
                RunAllSingleTests();
            } 
            catch (HttpRequestException httpe)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    System.Diagnostics.Debug.WriteLine("There were problems accessing the services.");
                    System.Diagnostics.Debug.WriteLine(httpe.StackTrace);

                    HttpErrorHappened = true;
                    ErrorMessage = httpe.StackTrace;
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                System.Console.Out.WriteLine(e.StackTrace);

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    HttpErrorHappened = false;
                    ErrorMessage = e.StackTrace;
                }
            }
        }

        /// <summary>
        /// Scenario : the data is ready as is to be tested.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testFullName"></param>
        /// <param name="testDescription"></param>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        public void CompareAndLog_Test<T>(EnumTestUnitNames testFullName, string testDescription, T oldValues, T newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.OldId, testFullName, testDescription);
            var compareStrategy = new CompareStrategyFactory((dynamic)oldValues, (dynamic)newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.OldId),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        /// <summary>
        /// Scenario : the data needs to be prepared before testing.
        /// The clock was started prior to the call of this method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testFullName"></param>
        /// <param name="testDescription"></param>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="watch"></param>
        public void CompareAndLog_Test<T>(EnumTestUnitNames testFullName, string testDescription, T oldValues, T newValues, Stopwatch watch)
        {
            var resultReport = new ResultReport(this.UserId, this.OldId, testFullName, testDescription);
            var compareStrategy = new CompareStrategyFactory((dynamic)oldValues, (dynamic)newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.OldId),
                                                this.BuildNewServiceURL(this.PageName),
                                                resultReport);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, IEnumerable<XElement> oldServiceData, string oldSingleStringPath, string newValue)
        {
            string oldValue = ParsingHelper.ParseSingleValue(oldServiceData, oldSingleStringPath);

            this.CompareAndLog_Test(testFullName, testDescription, oldValue, newValue);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, Dictionary<HashSet<string>,HashSet<string>> newAndOldValues, Stopwatch watch)
        {
            var resultReport = new ResultReport(this.UserId, this.OldId, testFullName, testDescription);
            var compareStrategy = new CompareStrategyFactory(newAndOldValues,resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.OldId),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }
    }
}
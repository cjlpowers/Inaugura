// REALLEADSREDIRECT.CPP - Implementation file for your Internet Server
//    RealLeadsRedirect Filter

#include "stdafx.h"
#include "RealLeadsRedirect.h"

//#define FILETRACE



///////////////////////////////////////////////////////////////////////
// The one and only CRealLeadsRedirectFilter object

CRealLeadsRedirectFilter theFilter;


///////////////////////////////////////////////////////////////////////
// CRealLeadsRedirectFilter implementation

CRealLeadsRedirectFilter::CRealLeadsRedirectFilter()
{
}

CRealLeadsRedirectFilter::~CRealLeadsRedirectFilter()
{
}

BOOL CRealLeadsRedirectFilter::GetFilterVersion(PHTTP_FILTER_VERSION pVer)
{
	// Call default implementation for initialization
	CHttpFilter::GetFilterVersion(pVer);

	// Clear the flags set by base class
	pVer->dwFlags &= ~SF_NOTIFY_ORDER_MASK;

	// Set the flags we are interested in
	pVer->dwFlags |= SF_NOTIFY_ORDER_HIGH | SF_NOTIFY_SECURE_PORT | SF_NOTIFY_NONSECURE_PORT
			 | SF_NOTIFY_PREPROC_HEADERS | SF_NOTIFY_END_OF_NET_SESSION;

	// Load description string
	TCHAR sz[SF_MAX_FILTER_DESC_LEN+1];
	ISAPIVERIFY(::LoadString(AfxGetResourceHandle(),
			IDS_FILTER, sz, SF_MAX_FILTER_DESC_LEN));
	_tcscpy(pVer->lpszFilterDesc, sz);
	return TRUE;
}

DWORD CRealLeadsRedirectFilter::OnPreprocHeaders(CHttpFilterContext* pCtxt,
	PHTTP_FILTER_PREPROC_HEADERS pHeaderInfo)
{
	int codeLength = 4;
	char buffer[256];
    DWORD buffSize = sizeof(buffer);
    BOOL bHeader = pHeaderInfo->GetHeader(pCtxt->m_pFC, "url",
                                              buffer, &buffSize); 

	#ifdef FILETRACE
	FILE* fp = fopen("C:\\trace.txt","at");
	#endif

	CString prefix = "/";
	//CString prefix = "/RealLeads/";
    CString urlString(buffer);

	

	#ifdef FILETRACE
	if(fp)
	{		
		fprintf(fp,"URL: %s\n",urlString);
	}
	#endif
	
	if(urlString.GetLength() == prefix.GetLength() + codeLength && urlString.Find(prefix) == 0)
	{
		CString propertyNumber = urlString.Mid(prefix.GetLength());

		if(propertyNumber.GetLength() == codeLength && IsNumeric(propertyNumber))
		{
			
			#ifdef FILETRACE
			if(fp)
				fprintf(fp,"#: %s\n",propertyNumber);
			#endif

			CString newUrl = prefix + "listing.aspx?code=";		
			newUrl += propertyNumber;
			
			#ifdef FILETRACE
			if(fp)
				fprintf(fp,"New Url: %s\n",newUrl);
			#endif

			char *newUrlString= newUrl.GetBuffer(urlString.GetLength());
			pHeaderInfo->SetHeader(pCtxt->m_pFC, "url", newUrlString);

			#ifdef FILETRACE
			if(fp)
			{
				fprintf(fp,"--------------------------------\n");
				fclose(fp);
			}
			#endif

			return SF_STATUS_REQ_HANDLED_NOTIFICATION;	
		}				
	}	 	

	#ifdef FILETRACE
	if(fp)
	{
		fprintf(fp,"--------------------------------\n");
		fclose(fp);
	}
	#endif

    //we want to leave this alone and let IIS handle it
    return SF_STATUS_REQ_NEXT_NOTIFICATION;

}

DWORD CRealLeadsRedirectFilter::OnEndOfNetSession(CHttpFilterContext* pCtxt)
{
	// TODO: React to this notification accordingly and
	// return the appropriate status code
	return SF_STATUS_REQ_NEXT_NOTIFICATION;
}

// Do not edit the following lines, which are needed by ClassWizard.
#if 0
BEGIN_MESSAGE_MAP(CRealLeadsRedirectFilter, CHttpFilter)
	//{{AFX_MSG_MAP(CRealLeadsRedirectFilter)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()
#endif	// 0

///////////////////////////////////////////////////////////////////////
// If your extension will not use MFC, you'll need this code to make
// sure the extension objects can find the resource handle for the
// module.  If you convert your extension to not be dependent on MFC,
// remove the comments arounn the following AfxGetResourceHandle()
// and DllMain() functions, as well as the g_hInstance global.

/****

static HINSTANCE g_hInstance;

HINSTANCE AFXISAPI AfxGetResourceHandle()
{
	return g_hInstance;
}

BOOL WINAPI DllMain(HINSTANCE hInst, ULONG ulReason,
					LPVOID lpReserved)
{
	if (ulReason == DLL_PROCESS_ATTACH)
	{
		g_hInstance = hInst;
	}

	return TRUE;
}

****/


BOOL CRealLeadsRedirectFilter::IsNumeric(const CString Str)
	{
	// Check each character of the string
		// If a character at a certain position is not a digit,
		// then the string is not a valid natural number
		for(int i = 0; i < Str.GetLength(); i++)
		{
			if( Str[i] < '0' || Str[i] > '9' )
			return FALSE;
		}
		return TRUE;
	}

#if !defined(AFX_REALLEADSREDIRECT_H__653DE8B6_C4FA_47B9_A4B2_F2176932144C__INCLUDED_)
#define AFX_REALLEADSREDIRECT_H__653DE8B6_C4FA_47B9_A4B2_F2176932144C__INCLUDED_

// REALLEADSREDIRECT.H - Header file for your Internet Server
//    RealLeadsRedirect Filter

#include "resource.h"


class CRealLeadsRedirectFilter : public CHttpFilter
{
public:
	CRealLeadsRedirectFilter();
	~CRealLeadsRedirectFilter();

// Overrides
	// ClassWizard generated virtual function overrides
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//{{AFX_VIRTUAL(CRealLeadsRedirectFilter)
	public:
	virtual BOOL GetFilterVersion(PHTTP_FILTER_VERSION pVer);
	virtual DWORD OnPreprocHeaders(CHttpFilterContext* pCtxt, PHTTP_FILTER_PREPROC_HEADERS pHeaderInfo);
	virtual DWORD OnEndOfNetSession(CHttpFilterContext* pCtxt);
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CRealLeadsRedirectFilter)
	//}}AFX_MSG

	private:
		BOOL IsNumeric(const CString Str);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_REALLEADSREDIRECT_H__653DE8B6_C4FA_47B9_A4B2_F2176932144C__INCLUDED)

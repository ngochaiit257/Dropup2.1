const EGASmartSearchConfigs = {
	settings: {
		isEnabled: false,
    // color
		colorBg: '#FFFFFF',
		colorBgHover: '#F7F7F7',
		colorLabelBg: '#F7F7F7',
		colorLabelText: '#A0A0A0',
		colorItemBorder: '#EEEEEE',
		colorItemTextTitle: '#000000',
		colorItemTextPrice: '#000000',
		colorItemTextCompareAtPrice: '#A7A7A7',
		colorItemTextSku: '#747474',
		colorItemTextDescription: '#000000',
		colorItemTextViewAll: '#0288D1',
		colorHeaderText: '#A0A0A0',
		colorLoading: '#fae300',
		// general
		searchAccuracy: 'all',
		isFixedSearch: 'absolute',
		searchWidth: '400px',
		searchHeight: '500px',
		searchView:	'list',
    
		// product
		productSortby: '&sortby=(sold_quantity:product=desc)',
		displayProductDescription: false,
		productLimit: 10,

		// article
		displayArticle: false,
		displayArticleDescription: false,
		articleLimit: 3
	},
	CONSTANT: {
		shopDomain: '',
		searchView: 'ega.smartsearch.json',
		textHeaderSectionTitle: 'Kết quả tìm kiếm cho ',
		textProductSectionTitle: 'Sản phẩm',
		textArticleSectionTitle: 'Bài viết',
		notFound: 'Không tìm thấy kết quả',
		textFrom: 'Từ ',
		textShowAll: 'Hiển thị sản phẩm có chứa '
	}
}
var EGASmartSearchRegister = {
	js: [
		"//theme.hstatic.net/1000253775/1000634044/14/ega.smartsearch.js?v=124"
	]
}
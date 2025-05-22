import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_all_promo_by_customer_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/create_customer_promotion_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/entities/promotion.dart';
import 'package:barbermanagemobile/presentation/screens/ExchangePromotionScreen.dart';

class PromotionScreen extends StatefulWidget {
  const PromotionScreen({super.key});

  @override
  _PromotionScreenState createState() => _PromotionScreenState();
}

class _PromotionScreenState extends State<PromotionScreen> {
  final GetAllPromoByCustomerUseCase _getAllPromoByCustomerUseCase =
      GetIt.instance<GetAllPromoByCustomerUseCase>();
  final CreateCustomerPromotionUseCase _createCustomerPromotionUseCase =
      GetIt.instance<CreateCustomerPromotionUseCase>();
  final GetCustomerByUserIDUseCase _getCustomerByUserIDUseCase =
      GetIt.instance<GetCustomerByUserIDUseCase>();

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const shadowColor = Color(0xFF3E2723);

  int? _customerId;
  String? _errorMessage;

  @override
  void initState() {
    super.initState();
    _loadCustomerId();
  }

  Future<void> _loadCustomerId() async {
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    final user = authProvider.user;

    if (user == null || int.tryParse(user.userId) == null) {
      setState(() {
        _errorMessage = 'Không tìm thấy thông tin người dùng';
      });
      return;
    }

    final userId = int.parse(user.userId);
    final result = await _getCustomerByUserIDUseCase.call(userId);
    result.fold(
      (failure) {
        setState(() {
          _errorMessage = 'Lỗi lấy thông tin khách hàng: $failure';
        });
      },
      (customer) {
        setState(() {
          _customerId = customer.customerID;
          _errorMessage = null;
        });
      },
    );
  }

  Future<void> _refreshPromotions() async {
    await Future.delayed(Duration(seconds: 1));
    setState(() {});
  }

  void _showSnackBar(BuildContext context, String message, {bool isError = false}) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(
          message,
          style: TextStyle(
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w500,
            color: textColor,
          ),
        ),
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        backgroundColor: isError ? Colors.redAccent : primaryColor,
        elevation: 10,
        duration: Duration(seconds: 2),
        padding: EdgeInsets.symmetric(horizontal: 16, vertical: 12),
      ),
    );
  }

  Future<void> _claimPromotion(int customerId, int promoId, String promoName) async {
    try {
      await _createCustomerPromotionUseCase.call(customerId, promoId, 'Active');
      _showSnackBar(context, 'Đã lấy mã: $promoName');
      setState(() {});
    } catch (e) {
      _showSnackBar(context, 'Lỗi lấy mã: $e', isError: true);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        title: Text(
          'Ưu đãi',
          style: TextStyle(
            color: textColor,
            fontFamily: 'Poppins',
            fontWeight: FontWeight.bold,
          ),
        ),
        backgroundColor: primaryColor,
        elevation: 0,
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: () => Navigator.pop(context),
        ),
        actions: [
          IconButton(
            icon: Icon(Icons.swap_horiz, color: textColor),
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => ExchangePromotionScreen()),
              );
            },
            tooltip: 'Trao đổi mã giảm giá',
          ),
        ],
      ),
      body: _customerId == null && _errorMessage != null
          ? Center(
              child: Text(
                _errorMessage!,
                style: TextStyle(
                  color: Colors.redAccent,
                  fontSize: 14,
                  fontFamily: 'Poppins',
                ),
              ),
            )
          : _customerId == null
              ? Center(
                  child: CircularProgressIndicator(
                    valueColor: AlwaysStoppedAnimation<Color>(accentColor),
                  ),
                )
              : RefreshIndicator(
                  onRefresh: _refreshPromotions,
                  color: accentColor,
                  child: SingleChildScrollView(
                    physics: BouncingScrollPhysics(),
                    child: FutureBuilder<List<Promotion>>(
                      future: _getAllPromoByCustomerUseCase.call(_customerId!),
                      builder: (context, snapshot) {
                        if (snapshot.connectionState == ConnectionState.waiting) {
                          return Center(
                            child: CircularProgressIndicator(
                              valueColor: AlwaysStoppedAnimation<Color>(accentColor),
                            ),
                          );
                        } else if (snapshot.hasError) {
                          return Center(
                            child: Text(
                              'Lỗi: ${snapshot.error}',
                              style: TextStyle(
                                color: Colors.redAccent,
                                fontSize: 14,
                                fontFamily: 'Poppins',
                              ),
                            ),
                          );
                        } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                          return Center(
                            child: Text(
                              'Không có mã giảm giá nào',
                              style: TextStyle(
                                color: textColor,
                                fontSize: 14,
                                fontFamily: 'Poppins',
                              ),
                            ),
                          );
                        }

                        final allPromotions = snapshot.data!;
                        final customerPromotions = allPromotions.where((promo) => promo.isAssociatedWithCustomer).toList();

                        return Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            _buildSectionTitle(context, 'Mã giảm giá của bạn'),
                            customerPromotions.isEmpty
                                ? Center(
                                    child: Padding(
                                      padding: const EdgeInsets.all(16.0),
                                      child: Text(
                                        'Bạn chưa có mã giảm giá nào',
                                        style: TextStyle(
                                          color: textColor,
                                          fontSize: 14,
                                          fontFamily: 'Poppins',
                                        ),
                                      ),
                                    ),
                                  )
                                : ListView.builder(
                                    shrinkWrap: true,
                                    physics: NeverScrollableScrollPhysics(),
                                    padding: EdgeInsets.all(16),
                                    itemCount: customerPromotions.length,
                                    itemBuilder: (context, index) {
                                      final promotion = customerPromotions[index];
                                      final promoCount = promotion.customerPromos?.isNotEmpty == true
                                          ? (promotion.customerPromos![0]['promoCount'] as int? ?? 0)
                                          : 0;
                                      return _buildPromotionCard(
                                        context,
                                        promotion,
                                        isCustomerOwned: true,
                                        promoCount: promoCount,
                                        onClaim: promotion.promoType == 'FreePromotion'
                                            ? () => _claimPromotion(_customerId!, promotion.promoID, promotion.promoName)
                                            : null,
                                      );
                                    },
                                  ),
                            _buildSectionTitle(context, 'Tất cả mã giảm giá'),
                            ListView.builder(
                              shrinkWrap: true,
                              physics: NeverScrollableScrollPhysics(),
                              padding: EdgeInsets.all(16),
                              itemCount: allPromotions.length,
                              itemBuilder: (context, index) {
                                final promotion = allPromotions[index];
                                final promoCount = promotion.isAssociatedWithCustomer && promotion.customerPromos?.isNotEmpty == true
                                    ? (promotion.customerPromos![0]['promoCount'] as int? ?? 0)
                                    : null;
                                return _buildPromotionCard(
                                  context,
                                  promotion,
                                  isCustomerOwned: promotion.isAssociatedWithCustomer,
                                  promoCount: promoCount,
                                  onClaim: promotion.promoType == 'FreePromotion'
                                      ? () => _claimPromotion(_customerId!, promotion.promoID, promotion.promoName)
                                      : null,
                                );
                              },
                            ),
                            SizedBox(height: 20),
                          ],
                        );
                      },
                    ),
                  ),
                ),
    );
  }

  Widget _buildSectionTitle(BuildContext context, String title) {
    return Padding(
      padding: const EdgeInsets.fromLTRB(16, 10, 16, 5),
      child: Text(
        title,
        style: TextStyle(
          fontSize: 18,
          fontWeight: FontWeight.bold,
          color: accentColor,
          fontFamily: 'Poppins',
        ),
      ),
    );
  }

  Widget _buildPromotionCard(
    BuildContext context,
    Promotion promotion, {
    required bool isCustomerOwned,
    int? promoCount,
    VoidCallback? onClaim,
  }) {
    final promoImage = promotion.promoImage.isNotEmpty ? promotion.promoImage : null;

    return Padding(
      padding: const EdgeInsets.only(bottom: 12),
      child: Card(
        elevation: 5,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
        child: Container(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [shadowColor, primaryColor.withOpacity(0.9)],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            borderRadius: BorderRadius.circular(15),
            border: Border.all(color: accentColor.withOpacity(0.2), width: 0.5),
          ),
          child: Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              ClipRRect(
                borderRadius: BorderRadius.horizontal(left: Radius.circular(15)),
                child: promoImage != null
                    ? Image.network(
                        promoImage,
                        width: 120,
                        height: 100,
                        fit: BoxFit.cover,
                        loadingBuilder: (context, child, loadingProgress) {
                          if (loadingProgress == null) return child;
                          return Center(
                            child: CircularProgressIndicator(
                              valueColor: AlwaysStoppedAnimation<Color>(accentColor),
                            ),
                          );
                        },
                        errorBuilder: (context, error, stackTrace) => Container(
                          width: 120,
                          height: 100,
                          color: Colors.grey.shade300,
                          child: Icon(
                            Icons.broken_image,
                            color: Colors.redAccent,
                            size: 30,
                          ),
                        ),
                      )
                    : Container(
                        width: 120,
                        height: 100,
                        color: Colors.grey.shade300,
                        child: Icon(
                          Icons.local_offer,
                          color: Colors.grey,
                          size: 30,
                        ),
                      ),
              ),
              Expanded(
                child: Padding(
                  padding: const EdgeInsets.all(12.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Expanded(
                            child: Text(
                              promotion.promoName,
                              style: TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                                color: textColor,
                                fontFamily: 'Poppins',
                              ),
                              maxLines: 1,
                              overflow: TextOverflow.ellipsis,
                            ),
                          ),
                          if (isCustomerOwned)
                            Container(
                              padding: EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                              decoration: BoxDecoration(
                                color: accentColor.withOpacity(0.3),
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Text(
                                'Đã sở hữu',
                                style: TextStyle(
                                  fontSize: 12,
                                  color: textColor,
                                  fontFamily: 'Poppins',
                                ),
                              ),
                            ),
                        ],
                      ),
                      SizedBox(height: 4),
                      Text(
                        promotion.promoDescription,
                        style: TextStyle(
                          fontSize: 12,
                          color: textColor.withOpacity(0.8),
                          fontFamily: 'Poppins',
                        ),
                        maxLines: 2,
                        overflow: TextOverflow.ellipsis,
                      ),
                      SizedBox(height: 8),
                      // Use Wrap to prevent overflow
                      Wrap(
                        spacing: 8, // Space between elements
                        runSpacing: 4, // Space between lines if wrapped
                        crossAxisAlignment: WrapCrossAlignment.center,
                        children: [
                          Text(
                            'Giảm ${promotion.promoDiscount.toStringAsFixed(0)}%',
                            style: TextStyle(
                              fontSize: 14,
                              fontWeight: FontWeight.w600,
                              color: accentColor,
                              fontFamily: 'Poppins',
                            ),
                          ),
                          if (isCustomerOwned && promoCount != null)
                            Text(
                              'Số lượng: $promoCount',
                              style: TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.w600,
                                color: accentColor,
                                fontFamily: 'Poppins',
                              ),
                            ),
                          if (onClaim != null)
                            ConstrainedBox(
                              constraints: BoxConstraints(
                                minWidth: 80, // Ensure the button has a minimum width
                              ),
                              child: ElevatedButton(
                                onPressed: onClaim,
                                style: ElevatedButton.styleFrom(
                                  backgroundColor: accentColor,
                                  padding: EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                                  elevation: 0,
                                ),
                                child: Text(
                                  'Lấy mã',
                                  style: TextStyle(
                                    fontSize: 12,
                                    fontWeight: FontWeight.w600,
                                    color: textColor,
                                    fontFamily: 'Poppins',
                                  ),
                                ),
                              ),
                            ),
                        ],
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
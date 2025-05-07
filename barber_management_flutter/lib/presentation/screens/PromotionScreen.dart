import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/domain/usecases/get_promotions_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_promotions_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/create_customer_promotion_use_case.dart';
import 'package:barbermanagemobile/domain/entities/promotion.dart';
import 'package:barbermanagemobile/domain/entities/customer_promotion.dart';

class PromotionScreen extends StatefulWidget {
  const PromotionScreen({super.key});

  @override
  _PromotionScreenState createState() => _PromotionScreenState();
}

class _PromotionScreenState extends State<PromotionScreen> {
  final GetPromotionsUseCase _getPromotionsUseCase = GetIt.instance<GetPromotionsUseCase>();
  final GetCustomerPromotionsUseCase _getCustomerPromotionsUseCase = GetIt.instance<GetCustomerPromotionsUseCase>();
  final CreateCustomerPromotionUseCase _createCustomerPromotionUseCase = GetIt.instance<CreateCustomerPromotionUseCase>();

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const shadowColor = Color(0xFF3E2723);

  Future<void> _refreshPromotions() async {
    await Future.delayed(Duration(seconds: 1));
    setState(() {});
  }

  void _showSnackBar(BuildContext context, String message, {bool isError = false}) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message, style: TextStyle(fontFamily: 'Poppins', fontWeight: FontWeight.w500, color: textColor)),
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
      setState(() {}); // Refresh to update customer promotions
    } catch (e) {
      _showSnackBar(context, 'Lỗi lấy mã: $e', isError: true);
    }
  }

  @override
  Widget build(BuildContext context) {
    const customerId = 1003; // Hardcoded for now, replace with dynamic ID later

    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        title: Text(
          'Ưu đãi',
          style: TextStyle(color: textColor, fontFamily: 'Poppins', fontWeight: FontWeight.bold),
        ),
        backgroundColor: primaryColor,
        elevation: 0,
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: RefreshIndicator(
        onRefresh: _refreshPromotions,
        color: accentColor,
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _buildSectionTitle(context, 'Mã giảm giá của bạn'),
              FutureBuilder<List<CustomerPromotion>>(
                future: _getCustomerPromotionsUseCase.call(customerId),
                builder: (context, snapshot) {
                  if (snapshot.connectionState == ConnectionState.waiting) {
                    return Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor)));
                  } else if (snapshot.hasError) {
                    return Center(child: Text('Lỗi: ${snapshot.error}', style: TextStyle(color: Colors.redAccent, fontSize: 14)));
                  } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                    return Center(child: Text('Bạn chưa có mã giảm giá nào', style: TextStyle(color: textColor, fontSize: 14)));
                  }

                  final customerPromotions = snapshot.data!;
                  return ListView.builder(
                    shrinkWrap: true,
                    physics: NeverScrollableScrollPhysics(),
                    padding: EdgeInsets.all(16),
                    itemCount: customerPromotions.length,
                    itemBuilder: (context, index) {
                      final promotion = customerPromotions[index];
                      return _buildPromotionCard(context, promotion, isCustomerOwned: true);
                    },
                  );
                },
              ),
              _buildSectionTitle(context, 'Tất cả mã giảm giá'),
              FutureBuilder<List<Promotion>>(
                future: _getPromotionsUseCase.call(),
                builder: (context, snapshot) {
                  if (snapshot.connectionState == ConnectionState.waiting) {
                    return Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor)));
                  } else if (snapshot.hasError) {
                    return Center(child: Text('Lỗi: ${snapshot.error}', style: TextStyle(color: Colors.redAccent, fontSize: 14)));
                  } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                    return Center(child: Text('Không có mã giảm giá nào', style: TextStyle(color: textColor, fontSize: 14)));
                  }

                  final allPromotions = snapshot.data!;
                  return FutureBuilder<List<CustomerPromotion>>(
                    future: _getCustomerPromotionsUseCase.call(customerId),
                    builder: (context, customerSnapshot) {
                      final customerPromoIds = customerSnapshot.hasData
                          ? customerSnapshot.data!.map((p) => p.promoID).toSet()
                          : <int>{};

                      return ListView.builder(
                        shrinkWrap: true,
                        physics: NeverScrollableScrollPhysics(),
                        padding: EdgeInsets.all(16),
                        itemCount: allPromotions.length,
                        itemBuilder: (context, index) {
                          final promotion = allPromotions[index];
                          final isCustomerOwned = customerPromoIds.contains(promotion.promoID);
                          return _buildPromotionCard(
                            context,
                            promotion,
                            isCustomerOwned: isCustomerOwned,
                            onClaim: () => _claimPromotion(customerId, promotion.promoID, promotion.promoName),
                          );
                        },
                      );
                    },
                  );
                },
              ),
              SizedBox(height: 20),
            ],
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
        style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold, color: accentColor, fontFamily: 'Poppins'),
      ),
    );
  }

  Widget _buildPromotionCard(
    BuildContext context,
    dynamic promotion,
    {required bool isCustomerOwned, VoidCallback? onClaim}) {
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
            children: [
              ClipRRect(
                borderRadius: BorderRadius.horizontal(left: Radius.circular(15)),
                child: Image.network(
                  promotion.promoImage,
                  width: 120,
                  height: 100,
                  fit: BoxFit.cover,
                  loadingBuilder: (context, child, loadingProgress) {
                    if (loadingProgress == null) return child;
                    return Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor)));
                  },
                  errorBuilder: (context, error, stackTrace) => Icon(Icons.broken_image, color: Colors.redAccent, size: 30),
                ),
              ),
              Expanded(
                child: Padding(
                  padding: const EdgeInsets.all(12.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
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
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
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
                          if (!isCustomerOwned)
                            ElevatedButton(
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
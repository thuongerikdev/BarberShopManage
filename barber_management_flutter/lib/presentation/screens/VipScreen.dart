import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/domain/usecases/get_vips_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/entities/vip.dart';
import 'package:barbermanagemobile/domain/entities/customer.dart';
import 'package:dartz/dartz.dart' as dartz;

class VipScreen extends StatefulWidget {
  final int userId;

  const VipScreen({super.key, required this.userId});

  @override
  _VipScreenState createState() => _VipScreenState();
}

class _VipScreenState extends State<VipScreen> {
  final GetVipsUseCase _getVipsUseCase = GetIt.instance<GetVipsUseCase>();
  final GetCustomerByUserIDUseCase _getCustomerByUserIDUseCase = GetIt.instance<GetCustomerByUserIDUseCase>();

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const shadowColor = Color(0xFF3E2723);

  Future<void> _refreshVips() async {
    await Future.delayed(Duration(seconds: 1));
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        title: Text(
          'Shine Member',
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
        onRefresh: _refreshVips,
        color: accentColor,
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Customer Progress Section
              FutureBuilder<dartz.Either<String, Customer>>(
                future: _getCustomerByUserIDUseCase.call(widget.userId),
                builder: (context, customerSnapshot) {
                  if (customerSnapshot.connectionState == ConnectionState.waiting) {
                    return Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor))),
                    );
                  } else if (customerSnapshot.hasError) {
                    return Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Text('Lỗi khi tải thông tin khách hàng: ${customerSnapshot.error}', style: TextStyle(color: Colors.redAccent)),
                    );
                  } else if (!customerSnapshot.hasData) {
                    return Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Text('Không có thông tin khách hàng', style: TextStyle(color: textColor)),
                    );
                  }

                  return customerSnapshot.data!.fold(
                    (error) => Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Text('Lỗi: $error', style: TextStyle(color: Colors.redAccent)),
                    ),
                    (customer) => FutureBuilder<List<Vip>>(
                      future: _getVipsUseCase.call(),
                      builder: (context, vipSnapshot) {
                        if (vipSnapshot.connectionState == ConnectionState.waiting) {
                          return Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor))),
                          );
                        } else if (vipSnapshot.hasError) {
                          return Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: Text('Lỗi khi tải danh sách VIP: ${vipSnapshot.error}', style: TextStyle(color: Colors.redAccent)),
                          );
                        } else if (!vipSnapshot.hasData || vipSnapshot.data!.isEmpty) {
                          return Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: Text('Không có gói VIP nào', style: TextStyle(color: textColor)),
                          );
                        }

                        final vips = vipSnapshot.data!;
                        // Find the current VIP level
                        Vip currentVip;
                        try {
                          currentVip = vips.firstWhere(
                            (vip) => vip.vipID == customer.vipID,
                            orElse: () => Vip(
                              vipID: 0,
                              vipType: 'Không xác định',
                              vipStatus: 'Không xác định',
                              vipCost: 0,
                              vipDiscount: 0,
                              vipImage: '',
                            ),
                          );
                        } catch (e) {
                          return Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: Text('Lỗi: Không tìm thấy VIP phù hợp: $e', style: TextStyle(color: Colors.redAccent)),
                          );
                        }

                        // Find the next VIP level (if any)
                        final sortedVips = List<Vip>.from(vips)..sort((a, b) => a.vipCost.compareTo(b.vipCost));
                        final currentVipIndex = sortedVips.indexWhere((vip) => vip.vipID == customer.vipID);
                        final nextVip = currentVipIndex < sortedVips.length - 1 && currentVipIndex != -1
                            ? sortedVips[currentVipIndex + 1]
                            : null;

                        // Calculate progress for totalSpent
                        final totalSpentProgress = customer.totalSpent / (nextVip?.vipCost.toDouble() ?? currentVip.vipCost.toDouble());
                        final totalSpentPercentage = (totalSpentProgress.clamp(0.0, 1.0) * 100).toStringAsFixed(0);

                        // Calculate progress for loyaltyPoints (assuming a max of 10,000,000 for simplicity)
                        const maxLoyaltyPoints = 10000000.0; // Adjust based on your logic
                        final loyaltyProgress = customer.loyaltyPoints / maxLoyaltyPoints;
                        final loyaltyPercentage = (loyaltyProgress.clamp(0.0, 1.0) * 100).toStringAsFixed(0);

                        return Padding(
                          padding: const EdgeInsets.all(16.0),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Tiến Độ Thành Viên',
                                style: TextStyle(
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold,
                                  color: textColor,
                                  fontFamily: 'Poppins',
                                ),
                              ),
                              SizedBox(height: 16),
                              // Total Spent Progress
                              Text(
                                'Tổng Chi Tiêu: ${customer.totalSpent.toStringAsFixed(0)} VNĐ',
                                style: TextStyle(
                                  fontSize: 16,
                                  color: textColor,
                                  fontFamily: 'Poppins',
                                ),
                              ),
                              SizedBox(height: 8),
                              LinearProgressIndicator(
                                value: totalSpentProgress.clamp(0.0, 1.0),
                                backgroundColor: shadowColor,
                                valueColor: AlwaysStoppedAnimation<Color>(accentColor),
                                minHeight: 10,
                                borderRadius: BorderRadius.circular(5),
                              ),
                              SizedBox(height: 4),
                              Text(
                                nextVip != null
                                    ? 'Còn ${nextVip.vipCost - customer.totalSpent.toInt()} VNĐ để đạt ${nextVip.vipType} ($totalSpentPercentage%)'
                                    : 'Bạn đã đạt cấp ${currentVip.vipType}!',
                                style: TextStyle(
                                  fontSize: 12,
                                  color: textColor.withOpacity(0.7),
                                  fontFamily: 'Poppins',
                                ),
                              ),
                              SizedBox(height: 16),
                              // Loyalty Points Progress
                              Text(
                                'Điểm Tích Lũy: ${customer.loyaltyPoints.toStringAsFixed(0)}',
                                style: TextStyle(
                                  fontSize: 16,
                                  color: textColor,
                                  fontFamily: 'Poppins',
                                ),
                              ),
                              SizedBox(height: 8),
                              LinearProgressIndicator(
                                value: loyaltyProgress.clamp(0.0, 1.0),
                                backgroundColor: shadowColor,
                                valueColor: AlwaysStoppedAnimation<Color>(accentColor),
                                minHeight: 10,
                                borderRadius: BorderRadius.circular(5),
                              ),
                              SizedBox(height: 4),
                              Text(
                                'Đạt $loyaltyPercentage% trong tổng ${maxLoyaltyPoints.toStringAsFixed(0)} điểm',
                                style: TextStyle(
                                  fontSize: 12,
                                  color: textColor.withOpacity(0.7),
                                  fontFamily: 'Poppins',
                                ),
                              ),
                              SizedBox(height: 24),
                            ],
                          ),
                        );
                      },
                    ),
                  );
                },
              ),
              // VIP List Section
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16.0),
                child: Text(
                  'Danh Sách Gói VIP',
                  style: TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                    color: textColor,
                    fontFamily: 'Poppins',
                  ),
                ),
              ),
              FutureBuilder<List<Vip>>(
                future: _getVipsUseCase.call(),
                builder: (context, snapshot) {
                  if (snapshot.connectionState == ConnectionState.waiting) {
                    return Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor)));
                  } else if (snapshot.hasError) {
                    return Center(child: Text('Lỗi: ${snapshot.error}', style: TextStyle(color: Colors.redAccent, fontSize: 14)));
                  } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                    return Center(child: Text('Không có gói VIP nào', style: TextStyle(color: textColor, fontSize: 14)));
                  }

                  final vips = snapshot.data!;
                  return ListView.builder(
                    shrinkWrap: true,
                    physics: NeverScrollableScrollPhysics(),
                    padding: EdgeInsets.all(16),
                    itemCount: vips.length,
                    itemBuilder: (context, index) {
                      final vip = vips[index];
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
                                    vip.vipImage,
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
                                          vip.vipType,
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
                                          'Giảm ${vip.vipDiscount}%',
                                          style: TextStyle(
                                            fontSize: 14,
                                            fontWeight: FontWeight.w600,
                                            color: accentColor,
                                            fontFamily: 'Poppins',
                                          ),
                                        ),
                                        SizedBox(height: 4),
                                        Text(
                                          '${vip.vipCost.toString()} VNĐ',
                                          style: TextStyle(
                                            fontSize: 12,
                                            color: textColor.withOpacity(0.8),
                                            fontFamily: 'Poppins',
                                          ),
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
                    },
                  );
                },
              ),
            ],
          ),
        ),
      ),
    );
  }
}
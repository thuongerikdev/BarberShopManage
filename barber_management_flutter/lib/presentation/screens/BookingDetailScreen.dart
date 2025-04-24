import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_service_detail_use_case.dart';

class BookingDetailScreen extends StatefulWidget {
  final int serviceID;

  const BookingDetailScreen({super.key, required this.serviceID});

  @override
  _BookingDetailScreenState createState() => _BookingDetailScreenState();
}

class _BookingDetailScreenState extends State<BookingDetailScreen> {
  final GetBookingServiceDetailUseCase _useCase =
      GetIt.instance<GetBookingServiceDetailUseCase>();
  List<Map<String, dynamic>>? _serviceDetails;

  @override
  void initState() {
    super.initState();
    _fetchServiceDetails();
  }

  Future<void> _fetchServiceDetails() async {
    try {
      final details = await _useCase.call(widget.serviceID);
      if (mounted) {
        setState(() {
          _serviceDetails = details;
        });
      }
    } catch (e) {
      if (mounted) {
        setState(() {
          _serviceDetails = null;
        });
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Lỗi: $e')),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: Color(0xFFEFEBE9)),
          onPressed: () => Navigator.of(context).pop(),
        ),
        title: Text(
          'Danh sách dịch vụ',
          style: TextStyle(
            fontFamily: 'Poppins',
            color: Color(0xFFEFEBE9),
            fontWeight: FontWeight.bold,
          ),
        ),
        backgroundColor: Color(0xFF4E342E),
        elevation: 0,
      ),
      backgroundColor: Color(0xFF212121),
      body: _serviceDetails == null
          ? Center(child: CircularProgressIndicator(color: Color(0xFF8D6E63)))
          : _serviceDetails!.isEmpty
              ? Center(
                  child: Text(
                    'Không tìm thấy dịch vụ',
                    style: TextStyle(
                      fontSize: 16,
                      color: Color(0xFFEFEBE9),
                      fontFamily: 'Poppins',
                    ),
                  ),
                )
              : Column(
                  children: [
                    Expanded(
                      child: ListView(
                        padding: EdgeInsets.all(16),
                        children: [
                          // Đoạn mô tả ở đầu, cuộn cùng danh sách
                          Padding(
                            padding: EdgeInsets.only(bottom: 16),
                            child: Text(
                              'Khám phá các dịch vụ cắt tóc và gội đầu chuyên nghiệp của chúng tôi, được thiết kế để mang lại trải nghiệm thư giãn và phong cách. Chọn dịch vụ phù hợp với bạn và đặt lịch ngay hôm nay!',
                              style: TextStyle(
                                fontSize: 16,
                                color: Color(0xFFEFEBE9),
                                fontFamily: 'Poppins',
                                height: 1.5,
                              ),
                              textAlign: TextAlign.center,
                            ),
                          ),
                          // Danh sách các ServiceDetail
                          ..._serviceDetails!.asMap().entries.map((entry) {
                            // final index = entry.key;
                            final serviceDetail = entry.value;
                            return Card(
                              margin: EdgeInsets.only(bottom: 16),
                              elevation: 2,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(15),
                              ),
                              color: Color(0xFF4E342E),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  // Hiển thị ảnh dịch vụ
                                  ClipRRect(
                                    borderRadius: BorderRadius.vertical(top: Radius.circular(15)),
                                    child: serviceDetail['servImage'] != null &&
                                            serviceDetail['servImage'].isNotEmpty
                                        ? Image.network(
                                            serviceDetail['servImage'],
                                            height: 200,
                                            width: double.infinity,
                                            fit: BoxFit.cover,
                                            loadingBuilder: (context, child, loadingProgress) {
                                              if (loadingProgress == null) return child;
                                              return Center(
                                                child: CircularProgressIndicator(
                                                  color: Color(0xFF8D6E63),
                                                ),
                                              );
                                            },
                                            errorBuilder: (context, error, stackTrace) {
                                              return Image.asset(
                                                'assets/images/placeholder_service.jpg',
                                                height: 200,
                                                width: double.infinity,
                                                fit: BoxFit.cover,
                                              );
                                            },
                                          )
                                        : Image.asset(
                                            'assets/images/placeholder_service.jpg',
                                            height: 200,
                                            width: double.infinity,
                                            fit: BoxFit.cover,
                                          ),
                                  ),
                                  Padding(
                                    padding: EdgeInsets.all(16),
                                    child: Column(
                                      crossAxisAlignment: CrossAxisAlignment.start,
                                      children: [
                                        Text(
                                          (serviceDetail['servName'] ?? 'Dịch vụ không tên')
                                              .toUpperCase(),
                                          style: TextStyle(
                                            fontSize: 20,
                                            fontWeight: FontWeight.bold,
                                            color: Color(0xFFEFEBE9),
                                            fontFamily: 'Poppins',
                                          ),
                                        ),
                                        SizedBox(height: 8),
                                        Text(
                                          serviceDetail['servDescription'] ?? 'Không có mô tả',
                                          style: TextStyle(
                                            fontSize: 14,
                                            color: Color(0xFF8D6E63),
                                            fontFamily: 'Poppins',
                                          ),
                                        ),
                                        SizedBox(height: 16),
                                        Text(
                                          'Giá: ${(serviceDetail['servPrice'] ?? 0).toString()} VNĐ',
                                          style: TextStyle(
                                            fontSize: 16,
                                            color: Color(0xFF8D6E63),
                                            fontFamily: 'Poppins',
                                          ),
                                        ),
                                        SizedBox(height: 8),
                                        Text(
                                          'Trạng thái: ${serviceDetail['servStatus'] ?? 'Không xác định'}',
                                          style: TextStyle(
                                            fontSize: 14,
                                            color: Color(0xFFEFEBE9),
                                            fontFamily: 'Poppins',
                                          ),
                                        ),
                                        SizedBox(height: 16),
                                        Text(
                                          'Lịch đặt',
                                          style: TextStyle(
                                            fontSize: 18,
                                            fontWeight: FontWeight.bold,
                                            color: Color(0xFF8D6E63),
                                            fontFamily: 'Poppins',
                                          ),
                                        ),
                                        SizedBox(height: 8),
                                        serviceDetail['bookingAppointments'].isEmpty
                                            ? Text(
                                                'Chưa có lịch đặt',
                                                style: TextStyle(
                                                  fontSize: 14,
                                                  color: Color(0xFFEFEBE9),
                                                  fontFamily: 'Poppins',
                                                ),
                                              )
                                            : GridView.builder(
                                                shrinkWrap: true,
                                                physics: NeverScrollableScrollPhysics(),
                                                gridDelegate:
                                                    SliverGridDelegateWithFixedCrossAxisCount(
                                                  crossAxisCount: 2,
                                                  crossAxisSpacing: 16,
                                                  mainAxisSpacing: 16,
                                                  childAspectRatio: 0.7,
                                                ),
                                                itemCount: serviceDetail['bookingAppointments']
                                                    .length,
                                                itemBuilder: (context, apptIndex) {
                                                  final appointment =
                                                      serviceDetail['bookingAppointments']
                                                          [apptIndex];
                                                  return Card(
                                                    elevation: 2,
                                                    shape: RoundedRectangleBorder(
                                                      borderRadius: BorderRadius.circular(15),
                                                    ),
                                                    color: Color(0xFF4E342E),
                                                    child: Column(
                                                      crossAxisAlignment:
                                                          CrossAxisAlignment.start,
                                                      children: [
                                                        ClipRRect(
                                                          borderRadius: BorderRadius.vertical(
                                                              top: Radius.circular(15)),
                                                          child: serviceDetail['servImage'] !=
                                                                      null &&
                                                                  serviceDetail['servImage']
                                                                      .isNotEmpty
                                                              ? Image.network(
                                                                  serviceDetail['servImage'],
                                                                  height: 120,
                                                                  width: double.infinity,
                                                                  fit: BoxFit.cover,
                                                                  loadingBuilder: (context,
                                                                      child, loadingProgress) {
                                                                    if (loadingProgress == null)
                                                                      return child;
                                                                    return Center(
                                                                      child:
                                                                          CircularProgressIndicator(
                                                                        color: Color(0xFF8D6E63),
                                                                      ),
                                                                    );
                                                                  },
                                                                  errorBuilder: (context, error,
                                                                      stackTrace) {
                                                                    return Image.asset(
                                                                      'assets/images/placeholder_service.jpg',
                                                                      height: 120,
                                                                      width: double.infinity,
                                                                      fit: BoxFit.cover,
                                                                    );
                                                                  },
                                                                )
                                                              : Image.asset(
                                                                  'assets/images/placeholder_service.jpg',
                                                                  height: 120,
                                                                  width: double.infinity,
                                                                  fit: BoxFit.cover,
                                                                ),
                                                        ),
                                                        Padding(
                                                          padding: EdgeInsets.all(8),
                                                          child: Column(
                                                            crossAxisAlignment:
                                                                CrossAxisAlignment.start,
                                                            children: [
                                                              Text(
                                                                'Lịch đặt #${apptIndex + 1}',
                                                                style: TextStyle(
                                                                  fontSize: 16,
                                                                  fontWeight: FontWeight.bold,
                                                                  color: Color(0xFFEFEBE9),
                                                                  fontFamily: 'Poppins',
                                                                ),
                                                              ),
                                                              SizedBox(height: 4),
                                                              Text(
                                                                appointment.toString(),
                                                                style: TextStyle(
                                                                  fontSize: 12,
                                                                  color: Color(0xFF8D6E63),
                                                                  fontFamily: 'Poppins',
                                                                ),
                                                                maxLines: 2,
                                                                overflow: TextOverflow.ellipsis,
                                                              ),
                                                              SizedBox(height: 8),
                                                              Row(
                                                                mainAxisAlignment:
                                                                    MainAxisAlignment.spaceBetween,
                                                                children: [
                                                                  Container(
                                                                    padding:
                                                                        EdgeInsets.symmetric(
                                                                            horizontal: 8,
                                                                            vertical: 4),
                                                                    decoration: BoxDecoration(
                                                                      color: Color(0xFF8D6E63)
                                                                          .withOpacity(0.2),
                                                                      borderRadius:
                                                                          BorderRadius.circular(
                                                                              12),
                                                                    ),
                                                                    child: Text(
                                                                      '45 Phút',
                                                                      style: TextStyle(
                                                                        fontSize: 12,
                                                                        color: Color(0xFFEFEBE9),
                                                                        fontFamily: 'Poppins',
                                                                      ),
                                                                    ),
                                                                  ),
                                                                  GestureDetector(
                                                                    onTap: () {
                                                                      // TODO: Thêm hành động chi tiết nếu cần
                                                                    },
                                                                    child: Row(
                                                                      children: [
                                                                        Text(
                                                                          'Tìm hiểu thêm',
                                                                          style: TextStyle(
                                                                            fontSize: 12,
                                                                            color:
                                                                                Color(0xFFEFEBE9),
                                                                            fontFamily: 'Poppins',
                                                                          ),
                                                                        ),
                                                                        Icon(
                                                                          Icons.arrow_forward,
                                                                          size: 16,
                                                                          color: Color(0xFFEFEBE9),
                                                                        ),
                                                                      ],
                                                                    ),
                                                                  ),
                                                                ],
                                                              ),
                                                            ],
                                                          ),
                                                        ),
                                                      ],
                                                    ),
                                                  );
                                                },
                                              ),
                                        SizedBox(height: 16),
                                        // Nút "Tìm hiểu thêm" với hành động hiển thị SnackBar
                                        GestureDetector(
                                          onTap: () {
                                            ScaffoldMessenger.of(context).showSnackBar(
                                              SnackBar(
                                                content: Text(
                                                  'Đã chọn ${serviceDetail['servName'] ?? 'Dịch vụ không tên'}',
                                                  style: TextStyle(
                                                    fontFamily: 'Poppins',
                                                    color: Color(0xFFEFEBE9),
                                                  ),
                                                ),
                                                backgroundColor: Color(0xFF4E342E),
                                                duration: Duration(seconds: 2),
                                              ),
                                            );
                                          },
                                          child: Container(
                                            width: double.infinity,
                                            padding: EdgeInsets.symmetric(vertical: 12),
                                            decoration: BoxDecoration(
                                              color: Color(0xFF8D6E63).withOpacity(0.2),
                                              borderRadius: BorderRadius.circular(8),
                                            ),
                                            child: Center(
                                              child: Text(
                                                'TÌM HIỂU THÊM',
                                                style: TextStyle(
                                                  fontSize: 16,
                                                  fontWeight: FontWeight.bold,
                                                  color: Color(0xFFEFEBE9),
                                                  fontFamily: 'Poppins',
                                                ),
                                              ),
                                            ),
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                ],
                              ),
                            );
                          }).toList(),
                        ],
                      ),
                    ),
                    // Nút "Đặt lịch ngay" ở cuối danh sách
                    Padding(
                      padding: EdgeInsets.all(16),
                      child: SizedBox(
                        width: double.infinity,
                        child: ElevatedButton(
                          onPressed: () {
                            // TODO: Thêm logic đặt lịch cho tất cả dịch vụ
                          },
                          style: ElevatedButton.styleFrom(
                            backgroundColor: Color(0xFF8D6E63), // Màu sáng hơn, dễ nhìn
                            padding: EdgeInsets.symmetric(vertical: 16),
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(8),
                            ),
                          ),
                          child: Text(
                            'ĐẶT LỊCH NGAY',
                            style: TextStyle(
                              fontSize: 16,
                              fontWeight: FontWeight.bold,
                              color: Color(0xFFEFEBE9), // Text trắng để dễ đọc
                              fontFamily: 'Poppins',
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
    );
  }
}
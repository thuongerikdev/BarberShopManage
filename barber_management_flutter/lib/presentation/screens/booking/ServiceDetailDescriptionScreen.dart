import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_service_detail_description_use_case.dart';
import 'package:barbermanagemobile/presentation/screens/booking/BookingScreen.dart'; // Import BookingScreen

class ServiceDetailDescriptionScreen extends StatefulWidget {
  final int serviceDetailID;

  const ServiceDetailDescriptionScreen({super.key, required this.serviceDetailID});

  @override
  _ServiceDetailDescriptionScreenState createState() => _ServiceDetailDescriptionScreenState();
}

class _ServiceDetailDescriptionScreenState extends State<ServiceDetailDescriptionScreen> {
  final GetBookingServiceDetailDescriptionUseCase _useCase =
      GetIt.instance<GetBookingServiceDetailDescriptionUseCase>();
  List<Map<String, dynamic>>? _serviceDescriptions;
  bool _isLoading = true;
  String? _error;

  // Color constants (unchanged)
  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);

  @override
  void initState() {
    super.initState();
    _fetchServiceDescriptions();
  }

  Future<void> _fetchServiceDescriptions() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final descriptions = await _useCase.call(widget.serviceDetailID);
      print('Fetched descriptions: $descriptions'); // Debug print
      if (mounted) {
        setState(() {
          _serviceDescriptions = descriptions;
          _isLoading = false;
        });
      }
    } catch (e) {
      print('Error fetching descriptions: $e'); // Debug print
      if (mounted) {
        setState(() {
          _error = 'Lỗi: $e';
          _isLoading = false;
        });
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi khi lấy mô tả dịch vụ: $e'),
            backgroundColor: primaryColor,
          ),
        );
      }
    }
  }

  // Function to navigate to BookingScreen
  void _navigateToBookingScreen() {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => BookingScreen(
          onBack: () {
            Navigator.pop(context); // Pop BookingScreen to return to ServiceDetailDescriptionScreen
          },
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: () => Navigator.of(context).pop(),
        ),
        title: Text(
          'QUY TRÌNH DỊCH VỤ',
          style: TextStyle(
            fontFamily: 'Poppins',
            color: textColor,
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        backgroundColor: primaryColor,
        elevation: 0,
      ),
      backgroundColor: backgroundColor,
      body: _isLoading
          ? Center(child: CircularProgressIndicator(color: accentColor))
          : _error != null
              ? Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(
                        _error!,
                        style: TextStyle(
                          fontSize: 16,
                          color: Colors.redAccent,
                          fontFamily: 'Poppins',
                        ),
                      ),
                      SizedBox(height: 16),
                      ElevatedButton(
                        onPressed: _fetchServiceDescriptions,
                        style: ElevatedButton.styleFrom(
                          backgroundColor: accentColor,
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                        ),
                        child: Text(
                          'Thử lại',
                          style: TextStyle(
                            fontSize: 16,
                            color: textColor,
                            fontFamily: 'Poppins',
                          ),
                        ),
                      ),
                    ],
                  ),
                )
              : _serviceDescriptions == null || _serviceDescriptions!.isEmpty
                  ? Center(
                      child: Text(
                        'Không tìm thấy mô tả dịch vụ',
                        style: TextStyle(
                          fontSize: 16,
                          color: textColor,
                          fontFamily: 'Poppins',
                        ),
                      ),
                    )
                  : Column(
                      children: [
                        // Main content with scrolling
                        Expanded(
                          child: SingleChildScrollView(
                            child: Padding(
                              padding: EdgeInsets.all(16),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  // Header Description
                                  Text(
                                    '11 bước giúp bạn trải nghiệm dịch vụ thư giãn nhất tại Long Công với dịch vụ chuyên nghiệp, giúp bạn cảm thấy dễ chịu và thoải mái sau mỗi lần sử dụng.',
                                    style: TextStyle(
                                      fontSize: 14,
                                      color: textColor,
                                      fontFamily: 'Poppins',
                                      height: 1.5,
                                    ),
                                    textAlign: TextAlign.center,
                                  ),
                                  SizedBox(height: 16),
                                  // Service Name and Price
                                  Center(
                                    child: Column(
                                      children: [
                                        Text(
                                          _serviceDescriptions![0]['servName']?.toUpperCase() ?? 'Dịch vụ không tên',
                                          style: TextStyle(
                                            fontSize: 24,
                                            fontWeight: FontWeight.bold,
                                            color: textColor,
                                            fontFamily: 'Poppins',
                                          ),
                                        ),
                                        SizedBox(height: 8),
                                        Text(
                                          'Giá: ${_serviceDescriptions![0]['servPrice']?.toString() ?? '0'} VNĐ',
                                          style: TextStyle(
                                            fontSize: 16,
                                            color: accentColor,
                                            fontFamily: 'Poppins',
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                  SizedBox(height: 16),
                                  // Grid of Steps
                                  GridView.builder(
                                    shrinkWrap: true,
                                    physics: NeverScrollableScrollPhysics(),
                                    gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                                      crossAxisCount: 3,
                                      crossAxisSpacing: 10,
                                      mainAxisSpacing: 10,
                                      childAspectRatio: 0.8,
                                    ),
                                    itemCount: _serviceDescriptions!.length,
                                    itemBuilder: (context, index) {
                                      final description = _serviceDescriptions![index];
                                      print('Building item $index: $description'); // Debug print
                                      return Card(
                                        elevation: 2,
                                        shape: RoundedRectangleBorder(
                                          borderRadius: BorderRadius.circular(10),
                                        ),
                                        color: primaryColor.withOpacity(0.8),
                                        child: Column(
                                          crossAxisAlignment: CrossAxisAlignment.center,
                                          children: [
                                            // Step Image
                                            ClipRRect(
                                              borderRadius: BorderRadius.vertical(top: Radius.circular(10)),
                                              child: description['servImage'] != null &&
                                                      description['servImage'].isNotEmpty
                                                  ? Image.network(
                                                      description['servImage'],
                                                      width: double.infinity,
                                                      fit: BoxFit.cover,
                                                      loadingBuilder: (context, child, loadingProgress) {
                                                        if (loadingProgress == null) return child;
                                                        return Center(
                                                          child: CircularProgressIndicator(
                                                            color: accentColor,
                                                          ),
                                                        );
                                                      },
                                                      errorBuilder: (context, error, stackTrace) {
                                                        print('Image load error for item $index: $error'); // Debug print
                                                        return Container(
                                                          color: primaryColor,
                                                          child: Icon(
                                                            Icons.broken_image,
                                                            color: Colors.redAccent,
                                                            size: 50,
                                                          ),
                                                        );
                                                      },
                                                    )
                                                  : Container(
                                                      color: primaryColor,
                                                      child: Icon(
                                                        Icons.broken_image,
                                                        color: Colors.redAccent,
                                                        size: 50,
                                                      ),
                                                    ),
                                            ),
                                            // Step Text
                                            Padding(
                                              padding: EdgeInsets.all(8),
                                              child: Flexible(
                                                child: Text(
                                                  description['servDesName'] ?? 'Không có tên',
                                                  style: TextStyle(
                                                    fontSize: 12,
                                                    color: textColor,
                                                    fontFamily: 'Poppins',
                                                    fontWeight: FontWeight.bold,
                                                  ),
                                                  textAlign: TextAlign.center,
                                                  maxLines: 2,
                                                  overflow: TextOverflow.ellipsis,
                                                ),
                                              ),
                                            ),
                                          ],
                                        ),
                                      );
                                    },
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ),
                        // Fixed "Đặt lịch ngay" button at the bottom
                        Padding(
                          padding: EdgeInsets.all(16),
                          child: SizedBox(
                            width: double.infinity,
                            child: ElevatedButton(
                              onPressed: _navigateToBookingScreen,
                              style: ElevatedButton.styleFrom(
                                backgroundColor: accentColor,
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
                                  color: textColor,
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
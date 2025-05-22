import 'package:barbermanagemobile/presentation/screens/vip/CheckInScreen.dart';
import 'package:carousel_slider/carousel_slider.dart' as carousel;
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_slider_images_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_services_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_blogs_use_case.dart';
import 'package:barbermanagemobile/domain/entities/social/blog.dart';
import 'package:barbermanagemobile/presentation/screens/booking/BookingDetailScreen.dart';
import 'package:barbermanagemobile/presentation/screens/booking/BookingHistoryScreen.dart';
import 'package:barbermanagemobile/presentation/screens/vip/PromotionScreen.dart';
import 'package:barbermanagemobile/presentation/screens/vip/VipScreen.dart';
import 'package:barbermanagemobile/presentation/screens/social/CommitmentScreen.dart';
import 'package:barbermanagemobile/presentation/screens/social/BranchScreen.dart';
import 'package:barbermanagemobile/presentation/screens/auth/EmployeeDetailScreen.dart';
import 'package:barbermanagemobile/presentation/screens/social/BlogDetailScreen.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  final GetSliderImagesUseCase _getSliderImagesUseCase =
      GetIt.instance<GetSliderImagesUseCase>();
  final GetEmployeesUseCase _getEmployeesUseCase =
      GetIt.instance<GetEmployeesUseCase>();
  final GetBookingServicesUseCase _getBookingServicesUseCase =
      GetIt.instance<GetBookingServicesUseCase>();
  final GetCustomerByUserIDUseCase _getCustomerByUserIDUseCase =
      GetIt.instance<GetCustomerByUserIDUseCase>();
  final GetBlogsUseCase _getBlogsUseCase = GetIt.instance<GetBlogsUseCase>();

  final List<Map<String, dynamic>> blocks = [
    {
      'title': 'Lịch sử đặt lịch',
      'icon': Icons.history,
      'color': Color(0xFFD7CCC8)
    },
    {'title': 'Ưu đãi', 'icon': Icons.local_offer, 'color': Color(0xFFBCAAA4)},
    {
      'title': 'Điểm danh',
      'icon': Icons.check_circle,
      'color': Color(0xFFA1887F)
    },
  ];

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const shadowColor = Color(0xFF3E2723);

  Future<void> _refreshData() async {
    await Future.delayed(Duration(seconds: 1));
    setState(() {});
    _showSnackBar(context, "Dữ liệu đã được làm mới");
  }

  Future<void> _navigateToBookingHistory(BuildContext context) async {
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    final user = authProvider.user;

    if (user?.userId != null && int.tryParse(user!.userId) != null) {
      final userIdInt = int.parse(user.userId);
      if (kDebugMode) {
        print('Navigating to BookingHistoryScreen with userId: $userIdInt');
      }
      try {
        final result = await _getCustomerByUserIDUseCase.call(userIdInt);
        result.fold(
          (error) => ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text("Lỗi: $error"),
              backgroundColor: primaryColor,
            ),
          ),
          (customer) {
            if (kDebugMode) {
              print(
                  'Navigating to BookingHistoryScreen with custID: ${customer.customerID}');
            }
            Navigator.push(
              context,
              MaterialPageRoute(
                builder: (context) =>
                    BookingHistoryScreen(custID: customer.customerID),
              ),
            );
          },
        );
      } catch (e) {
        if (kDebugMode) {
          print('Error fetching customer: $e');
        }
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text("Lỗi khi lấy thông tin khách hàng: $e"),
            backgroundColor: primaryColor,
          ),
        );
      }
    } else {
      if (kDebugMode) {
        print('Invalid userId: ${user?.userId}');
      }
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text("Không tìm thấy thông tin người dùng"),
          backgroundColor: primaryColor,
        ),
      );
    }
  }

  Future<void> _navigateToVipScreen(BuildContext context) async {
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    final user = authProvider.user;

    if (user?.userId != null && int.tryParse(user!.userId) != null) {
      final userIdInt = int.parse(user.userId);
      if (kDebugMode) {
        print('Navigating to VipScreen with userId: $userIdInt');
      }
      Navigator.push(
        context,
        MaterialPageRoute(
          builder: (context) => VipScreen(userId: userIdInt),
        ),
      );
    } else {
      if (kDebugMode) {
        print('Invalid userId: ${user?.userId}');
      }
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text("Không tìm thấy thông tin người dùng"),
          backgroundColor: primaryColor,
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      body: RefreshIndicator(
        onRefresh: _refreshData,
        color: accentColor,
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Padding(
                padding:
                    const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceAround,
                  children: [
                    GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => PromotionScreen()),
                        );
                      },
                      child: _buildQuickActionButton(
                          Icons.local_offer, "Ưu đãi", accentColor),
                    ),
                    GestureDetector(
                      onTap: () => _navigateToVipScreen(context),
                      child: _buildQuickActionButton(
                          Icons.star, "Shine Member", accentColor),
                    ),
                    GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => CommitmentScreen()),
                        );
                      },
                      child: _buildQuickActionButton(
                          Icons.security, "Cam kết", accentColor),
                    ),
                    GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => BranchScreen()),
                        );
                      },
                      child: _buildQuickActionButton(
                          Icons.web, "Hệ thống", accentColor),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 10),
                child: SizedBox(
                  height: 150,
                  width: double.infinity,
                  child: FutureBuilder<List<Map<String, String>>>(
                    future: _getSliderImagesUseCase.call(),
                    builder: (context, snapshot) {
                      if (snapshot.connectionState == ConnectionState.waiting) {
                        return Center(
                            child: CircularProgressIndicator(
                                valueColor: AlwaysStoppedAnimation<Color>(
                                    accentColor)));
                      } else if (snapshot.hasError) {
                        return Center(
                            child: Text('Lỗi: ${snapshot.error}',
                                style: TextStyle(
                                    color: Colors.redAccent, fontSize: 14)));
                      } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                        return Center(
                            child: Text('Không có dữ liệu ảnh',
                                style:
                                    TextStyle(color: textColor, fontSize: 14)));
                      }

                      final sliderItems = snapshot.data!;
                      return carousel.CarouselSlider(
                        options: carousel.CarouselOptions(
                          autoPlay: true,
                          autoPlayInterval: Duration(seconds: 3),
                          aspectRatio: 16 / 9,
                          enlargeCenterPage: true,
                          viewportFraction: 1.0,
                        ),
                        items: sliderItems.map((item) {
                          return Container(
                            margin: EdgeInsets.zero,
                            decoration: BoxDecoration(
                              borderRadius: BorderRadius.circular(15),
                              boxShadow: [
                                BoxShadow(
                                    color: shadowColor.withOpacity(0.3),
                                    blurRadius: 10,
                                    offset: Offset(0, 5))
                              ],
                            ),
                            child: ClipRRect(
                              borderRadius: BorderRadius.circular(15),
                              child: Stack(
                                fit: StackFit.expand,
                                children: [
                                  Image.network(
                                    item['image']!,
                                    fit: BoxFit.cover,
                                    loadingBuilder:
                                        (context, child, loadingProgress) {
                                      if (loadingProgress == null) return child;
                                      return Center(
                                          child: CircularProgressIndicator(
                                              valueColor:
                                                  AlwaysStoppedAnimation<Color>(
                                                      accentColor)));
                                    },
                                    errorBuilder:
                                        (context, error, stackTrace) => Center(
                                            child: Icon(Icons.error,
                                                color: Colors.redAccent)),
                                  ),
                                  Container(
                                    decoration: BoxDecoration(
                                      gradient: LinearGradient(
                                        colors: [
                                          shadowColor.withOpacity(0.5),
                                          Colors.transparent
                                        ],
                                        begin: Alignment.bottomCenter,
                                        end: Alignment.topCenter,
                                      ),
                                    ),
                                  ),
                                  Positioned(
                                    bottom: 10,
                                    left: 10,
                                    child: Text(
                                      item['title']!,
                                      style: TextStyle(
                                          color: textColor,
                                          fontSize: 14,
                                          fontWeight: FontWeight.bold,
                                          fontFamily: 'Poppins'),
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          );
                        }).toList(),
                      );
                    },
                  ),
                ),
              ),
              _buildSectionTitle(context, "Dịch vụ nổi bật"),
              Container(
                height: 180,
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: FutureBuilder<List<Map<String, dynamic>>>(
                  future: _getBookingServicesUseCase.call(),
                  builder: (context, snapshot) {
                    if (snapshot.connectionState == ConnectionState.waiting) {
                      return Center(
                          child: CircularProgressIndicator(
                              valueColor:
                                  AlwaysStoppedAnimation<Color>(accentColor)));
                    } else if (snapshot.hasError) {
                      return Center(
                          child: Text('Lỗi: ${snapshot.error}',
                              style: TextStyle(
                                  color: Colors.redAccent, fontSize: 14)));
                    } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                      return Center(
                          child: Text('Không có dữ liệu dịch vụ',
                              style:
                                  TextStyle(color: textColor, fontSize: 14)));
                    }

                    final services = snapshot.data!;
                    return ListView.builder(
                      scrollDirection: Axis.horizontal,
                      physics: BouncingScrollPhysics(),
                      itemCount: services.length,
                      itemBuilder: (context, index) {
                        final servImage = services[index]['servImage']
                                    ?.toString()
                                    .isNotEmpty ==
                                true
                            ? services[index]['servImage']
                            : 'assets/images/placeholder_service.jpg';
                        final servDescription = services[index]
                                        ['servDescription']
                                    ?.toString()
                                    .isNotEmpty ==
                                true
                            ? services[index]['servDescription']
                            : null;
                        final serviceName =
                            services[index]['servName']?.toString() ??
                                'Dịch vụ không tên';
                        final servicePrice =
                            services[index]['servPrice']?.toString() ?? '0';
                        final serviceID =
                            services[index]['servID']?.toString() ?? '';

                        return Padding(
                          padding: const EdgeInsets.only(right: 10),
                          child: InkWell(
                            onTap: () {
                              if (serviceID.isEmpty ||
                                  int.tryParse(serviceID) == null) {
                                _showSnackBar(context,
                                    "Dịch vụ không hợp lệ (ID không hợp lệ)");
                                return;
                              }
                              Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (context) => BookingDetailScreen(
                                      serviceID: int.parse(serviceID)),
                                ),
                              );
                            },
                            borderRadius: BorderRadius.circular(15),
                            child: Card(
                              elevation: 5,
                              shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(15)),
                              child: Container(
                                width: 120,
                                decoration: BoxDecoration(
                                  gradient: LinearGradient(
                                    colors: [
                                      shadowColor.withOpacity(0.8),
                                      primaryColor
                                    ],
                                    begin: Alignment.topLeft,
                                    end: Alignment.bottomRight,
                                  ),
                                  borderRadius: BorderRadius.circular(15),
                                  border: Border.all(
                                      color: accentColor.withOpacity(0.2),
                                      width: 0.5),
                                ),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.center,
                                  children: [
                                    Expanded(
                                      child: ClipRRect(
                                        borderRadius: BorderRadius.vertical(
                                            top: Radius.circular(15)),
                                        child: servImage.startsWith('http')
                                            ? Image.network(
                                                servImage,
                                                width: double.infinity,
                                                fit: BoxFit.cover,
                                                loadingBuilder: (context, child,
                                                    loadingProgress) {
                                                  if (loadingProgress == null)
                                                    return child;
                                                  return Center(
                                                      child: CircularProgressIndicator(
                                                          valueColor:
                                                              AlwaysStoppedAnimation<
                                                                      Color>(
                                                                  accentColor)));
                                                },
                                                errorBuilder: (context, error,
                                                        stackTrace) =>
                                                    Image.asset(
                                                  'assets/images/placeholder_service.jpg',
                                                  width: double.infinity,
                                                  fit: BoxFit.cover,
                                                ),
                                              )
                                            : Image.asset(
                                                servImage,
                                                width: double.infinity,
                                                fit: BoxFit.cover,
                                              ),
                                      ),
                                    ),
                                    Padding(
                                      padding: const EdgeInsets.all(8.0),
                                      child: Column(
                                        children: [
                                          Text(
                                            servDescription ?? serviceName,
                                            style: TextStyle(
                                              fontSize: 12,
                                              fontWeight: FontWeight.w600,
                                              color: textColor,
                                              fontFamily: 'Poppins',
                                            ),
                                            textAlign: TextAlign.center,
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                          ),
                                          SizedBox(height: 4),
                                          Text(
                                            '$servicePrice VNĐ',
                                            style: TextStyle(
                                              fontSize: 10,
                                              color: accentColor,
                                              fontFamily: 'Poppins',
                                            ),
                                          ),
                                        ],
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ),
                          ),
                        );
                      },
                    );
                  },
                ),
              ),
              _buildSectionTitle(context, "Nhân viên"),
              Container(
                height: 180,
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: FutureBuilder<List<Map<String, String>>>(
                  future: _getEmployeesUseCase.call(),
                  builder: (context, snapshot) {
                    if (snapshot.connectionState == ConnectionState.waiting) {
                      return Center(
                        child: CircularProgressIndicator(
                          valueColor:
                              AlwaysStoppedAnimation<Color>(accentColor),
                        ),
                      );
                    } else if (snapshot.hasError) {
                      return Center(
                        child: Text(
                          'Lỗi: ${snapshot.error}',
                          style: TextStyle(
                              color: Colors.redAccent, fontSize: 14),
                        ),
                      );
                    } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                      return Center(
                        child: Text(
                          'Không có dữ liệu nhân viên',
                          style: TextStyle(
                              color: textColor, fontSize: 14),
                        ),
                      );
                    }

                    final employees = snapshot.data!;
                    if (kDebugMode) {
                      print('Employees data: $employees');
                    }

                    return ListView.builder(
                      scrollDirection: Axis.horizontal,
                      physics: BouncingScrollPhysics(),
                      itemCount: employees.length,
                      itemBuilder: (context, index) {
                        final employee = employees[index];
                        final userID = int.tryParse(employee['userID'] ?? '');
                        final name = employee['name'] ?? 'Unknown';
                        final role = employee['role'] ?? 'Nhân viên';
                        if (kDebugMode) {
                          print('Employee at index $index: $employee');
                          print(
                              'User ID at index $index: ${employee['userID']} (parsed: $userID)');
                        }

                        if (userID == null) {
                          print('Invalid userID for employee: $employee');
                          return SizedBox.shrink();
                        }

                        return Padding(
                          padding: const EdgeInsets.only(right: 10),
                          child: InkWell(
                            onTap: () {
                              print(
                                  'Navigating to EmployeeDetailScreen with userID: $userID');
                              Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (context) =>
                                      EmployeeDetailScreen(userID: userID),
                                ),
                              );
                            },
                            borderRadius: BorderRadius.circular(15),
                            child: Card(
                              elevation: 5,
                              shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(15)),
                              child: Container(
                                width: 120,
                                decoration: BoxDecoration(
                                  gradient: LinearGradient(
                                    colors: [
                                      shadowColor.withOpacity(0.8),
                                      primaryColor
                                    ],
                                    begin: Alignment.topLeft,
                                    end: Alignment.bottomRight,
                                  ),
                                  borderRadius: BorderRadius.circular(15),
                                  border: Border.all(
                                      color: accentColor.withOpacity(0.2),
                                      width: 0.5),
                                ),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.center,
                                  children: [
                                    Expanded(
                                      child: ClipRRect(
                                        borderRadius: BorderRadius.vertical(
                                            top: Radius.circular(15)),
                                        child: Image.network(
                                          employee['image'] ??
                                              'assets/images/placeholder_employee.jpg',
                                          width: double.infinity,
                                          fit: BoxFit.cover,
                                          loadingBuilder: (context, child,
                                              loadingProgress) {
                                            if (loadingProgress == null)
                                              return child;
                                            return Center(
                                              child: CircularProgressIndicator(
                                                valueColor:
                                                    AlwaysStoppedAnimation<
                                                        Color>(accentColor),
                                              ),
                                            );
                                          },
                                          errorBuilder:
                                              (context, error, stackTrace) =>
                                                  Image.asset(
                                            'assets/images/placeholder_employee.jpg',
                                            width: double.infinity,
                                            fit: BoxFit.cover,
                                          ),
                                        ),
                                      ),
                                    ),
                                    Padding(
                                      padding: const EdgeInsets.all(8.0),
                                      child: Column(
                                        children: [
                                          Text(
                                            name,
                                            style: TextStyle(
                                              fontSize: 12,
                                              fontWeight: FontWeight.w600,
                                              color: textColor,
                                              fontFamily: 'Poppins',
                                            ),
                                            textAlign: TextAlign.center,
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                          ),
                                          SizedBox(height: 4),
                                          Text(
                                            role,
                                            style: TextStyle(
                                              fontSize: 10,
                                              color: accentColor,
                                              fontFamily: 'Poppins',
                                            ),
                                            textAlign: TextAlign.center,
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis,
                                          ),
                                        ],
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ),
                          ),
                        );
                      },
                    );
                  },
                ),
              ),
              _buildSectionTitle(context, "Danh Sách Blog"),
              Container(
                height: 180,
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: FutureBuilder<List<Blog>>(
                  future: _getBlogsUseCase.call(),
                  builder: (context, snapshot) {
                    if (snapshot.connectionState == ConnectionState.waiting) {
                      return Center(
                          child: CircularProgressIndicator(
                              valueColor:
                                  AlwaysStoppedAnimation<Color>(accentColor)));
                    } else if (snapshot.hasError) {
                      return Center(
                          child: Text('Lỗi: ${snapshot.error}',
                              style: TextStyle(
                                  color: Colors.redAccent, fontSize: 14)));
                    } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                      return Center(
                          child: Text('Không có dữ liệu blog',
                              style:
                                  TextStyle(color: textColor, fontSize: 14)));
                    }

                    final blogs = snapshot.data!;
                    return ListView.builder(
                      scrollDirection: Axis.horizontal,
                      physics: BouncingScrollPhysics(),
                      itemCount: blogs.length,
                      itemBuilder: (context, index) {
                        final blog = blogs[index];
                        final blogImage = blog.blogImage.isNotEmpty
                            ? blog.blogImage
                            : 'https://via.placeholder.com/120';
                        final blogTitle = blog.blogTitle.isNotEmpty
                            ? blog.blogTitle
                            : 'Không có tiêu đề';
                        final blogDescription = blog.blogDescription.isNotEmpty
                            ? blog.blogDescription
                            : 'Không có mô tả';

                        return Padding(
                          padding: const EdgeInsets.only(right: 10),
                          child: InkWell(
                            onTap: () {
                              Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (context) =>
                                      BlogDetailScreen(blogId: blog.blogID),
                                ),
                              );
                            },
                            borderRadius: BorderRadius.circular(15),
                            child: Card(
                              elevation: 5,
                              shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(15)),
                              child: Container(
                                width: 200,
                                decoration: BoxDecoration(
                                  gradient: LinearGradient(
                                    colors: [
                                      shadowColor.withOpacity(0.8),
                                      primaryColor
                                    ],
                                    begin: Alignment.topLeft,
                                    end: Alignment.bottomRight,
                                  ),
                                  borderRadius: BorderRadius.circular(15),
                                  border: Border.all(
                                      color: accentColor.withOpacity(0.2),
                                      width: 0.5),
                                ),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Expanded(
                                      child: ClipRRect(
                                        borderRadius: BorderRadius.vertical(
                                            top: Radius.circular(15)),
                                        child: Image.network(
                                          blogImage,
                                          width: double.infinity,
                                          fit: BoxFit.cover,
                                          loadingBuilder: (context, child,
                                              loadingProgress) {
                                            if (loadingProgress == null)
                                              return child;
                                            return Center(
                                                child: CircularProgressIndicator(
                                                    valueColor:
                                                        AlwaysStoppedAnimation<
                                                                Color>(
                                                            accentColor)));
                                          },
                                          errorBuilder:
                                              (context, error, stackTrace) =>
                                                  Icon(Icons.broken_image,
                                                      color: Colors.redAccent,
                                                      size: 30),
                                        ),
                                      ),
                                    ),
                                    Padding(
                                      padding: const EdgeInsets.all(8.0),
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            blogTitle,
                                            style: TextStyle(
                                              fontSize: 12,
                                              fontWeight: FontWeight.w600,
                                              color: textColor,
                                              fontFamily: 'Poppins',
                                            ),
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis,
                                          ),
                                          SizedBox(height: 4),
                                          Text(
                                            blogDescription,
                                            style: TextStyle(
                                              fontSize: 10,
                                              color: textColor.withOpacity(0.7),
                                              fontFamily: 'Poppins',
                                            ),
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                          ),
                                        ],
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ),
                          ),
                        );
                      },
                    );
                  },
                ),
              ),
              _buildSectionTitle(context, "Tiện ích"),
              Padding(
                padding:
                    const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  children: blocks.map((block) {
                    final title = block['title'] as String;
                    return GestureDetector(
                      onTap: () {
                        if (title == 'Lịch sử đặt lịch') {
                          _navigateToBookingHistory(context);
                        } else if (title == 'Ưu đãi') {
                          Navigator.push(
                            context,
                            MaterialPageRoute(
                                builder: (context) => PromotionScreen()),
                          );
                        } else if (title == 'Điểm danh') {
                          Navigator.push(
                            context,
                            MaterialPageRoute(
                                builder: (context) => CheckInScreen()),
                          );
                        }
                      },
                      child: _buildUtilityButton(title,
                          block['icon'] as IconData, block['color'] as Color),
                    );
                  }).toList(),
                ),
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
        style: TextStyle(
            fontSize: 18,
            fontWeight: FontWeight.bold,
            color: accentColor,
            fontFamily: 'Poppins'),
      ),
    );
  }

  Widget _buildQuickActionButton(IconData icon, String label, Color color) {
    return Column(
      children: [
        CircleAvatar(
            radius: 18,
            backgroundColor: color.withOpacity(0.1),
            child: Icon(icon, color: color, size: 18)),
        SizedBox(height: 5),
        Text(label,
            style: TextStyle(
                fontSize: 11, color: textColor, fontFamily: 'Poppins')),
      ],
    );
  }

  Widget _buildUtilityButton(String title, IconData icon, Color color) {
    return Column(
      children: [
        CircleAvatar(
            radius: 18,
            backgroundColor: color.withOpacity(0.1),
            child: Icon(icon, color: color, size: 18)),
        SizedBox(height: 5),
        Text(title,
            style: TextStyle(
                fontFamily: 'Poppins', fontSize: 11, color: textColor)),
      ],
    );
  }

  void _showSnackBar(BuildContext context, String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(
          message,
          style: TextStyle(
              fontFamily: 'Poppins',
              fontWeight: FontWeight.w500,
              color: textColor),
        ),
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        backgroundColor: primaryColor,
        elevation: 10,
        duration: Duration(seconds: 2),
        padding: EdgeInsets.symmetric(horizontal: 16, vertical: 12),
      ),
    );
  }
}
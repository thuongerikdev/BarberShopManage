import 'package:carousel_slider/carousel_slider.dart' as carousel;
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../providers/auth_provider.dart';
import 'login_screen.dart';

class MainScreen extends StatefulWidget {
  @override
  _MainScreenState createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> with SingleTickerProviderStateMixin {
  late AnimationController _controller;
  late Animation<double> _fadeAnimation;

  @override
  void initState() {
    super.initState();
    _controller = AnimationController(
      duration: const Duration(milliseconds: 1000),
      vsync: this,
    );
    _fadeAnimation = CurvedAnimation(parent: _controller, curve: Curves.easeIn);
    _controller.forward();
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  // Dữ liệu giả lập
  final List<Map<String, String>> sliderItems = [
    {'image': 'https://via.placeholder.com/600x200?text=Khuyến+mãi+Cắt+tóc', 'title': 'Ưu đãi 50% Cắt tóc'},
    {'image': 'https://via.placeholder.com/600x200?text=Nhuộm+tóc+chuyên+nghiệp', 'title': 'Nhuộm tóc chuyên nghiệp'},
    {'image': 'https://via.placeholder.com/600x200?text=Massage+thư+giãn', 'title': 'Massage thư giãn'},
  ];

  final List<Map<String, String>> services = [
    {'name': 'Cắt tóc', 'image': 'https://via.placeholder.com/100?text=Cắt+tóc'},
    {'name': 'Nhuộm tóc', 'image': 'https://via.placeholder.com/100?text=Nhuộm'},
    {'name': 'Massage', 'image': 'https://via.placeholder.com/100?text=Massage'},
  ];

  final List<Map<String, String>> employees = [
    {'name': 'Nguyễn Văn A', 'image': 'https://via.placeholder.com/100?text=A'},
    {'name': 'Trần Thị B', 'image': 'https://via.placeholder.com/100?text=B'},
    {'name': 'Lê Văn C', 'image': 'https://via.placeholder.com/100?text=C'},
  ];

  final List<Map<String, dynamic>> blocks = [
    {'title': 'Lịch sử đặt lịch', 'icon': Icons.history, 'color': Colors.blue},
    {'title': 'Ưu đãi', 'icon': Icons.local_offer, 'color': Colors.orange},
    {'title': 'Hỗ trợ', 'icon': Icons.support_agent, 'color': Colors.green},
  ];

  // Bảng màu
  static const primaryColor = Color(0xFF1E88E5); // Xanh chính
  // static const accentColor = Color(0xFFFFA726); // Cam nổi bật
  static const backgroundColor = Color(0xFFF5F7FA); // Nền nhẹ

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context);
    final user = authProvider.user;

    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        flexibleSpace: Container(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [primaryColor, primaryColor.withOpacity(0.7)],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
        ),
        title: Row(
          children: [
            Icon(Icons.cut, color: Colors.white, size: 28),
            SizedBox(width: 8),
            Text(
              "Barber Booking",
              style: TextStyle(
                fontFamily: 'Roboto',
                fontWeight: FontWeight.w800,
                fontSize: 24,
                color: Colors.white,
                letterSpacing: 1.2,
              ),
            ),
          ],
        ),
        actions: [
          IconButton(
            icon: Icon(Icons.logout, color: Colors.white),
            onPressed: () {
              authProvider.logout();
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => LoginScreen()),
              );
            },
          ),
        ],
      ),
      body: FadeTransition(
        opacity: _fadeAnimation,
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Slider khuyến mãi
              Container(
                height: 240,
                margin: EdgeInsets.symmetric(vertical: 16),
                child: carousel.CarouselSlider(
                  options: carousel.CarouselOptions(
                    autoPlay: true,
                    autoPlayInterval: Duration(seconds: 3),
                    aspectRatio: 16 / 9,
                    enlargeCenterPage: true,
                    viewportFraction: 0.9,
                    enlargeStrategy: carousel.CenterPageEnlargeStrategy.scale,
                  ),
                  items: sliderItems.map((item) {
                    return GestureDetector(
                      onTap: () {
                        ScaffoldMessenger.of(context).showSnackBar(
                          SnackBar(content: Text("Đã chọn ${item['title']}")),
                        );
                      },
                      child: Container(
                        margin: EdgeInsets.symmetric(horizontal: 8),
                        decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(24),
                          boxShadow: [
                            BoxShadow(
                              color: Colors.black.withOpacity(0.15),
                              blurRadius: 12,
                              offset: Offset(0, 6),
                            ),
                          ],
                        ),
                        child: ClipRRect(
                          borderRadius: BorderRadius.circular(24),
                          child: Stack(
                            fit: StackFit.expand,
                            children: [
                              Image.network(
                                item['image']!,
                                fit: BoxFit.cover,
                                loadingBuilder: (context, child, loadingProgress) {
                                  if (loadingProgress == null) return child;
                                  return Center(
                                    child: CircularProgressIndicator(
                                      valueColor: AlwaysStoppedAnimation<Color>(primaryColor),
                                    ),
                                  );
                                },
                              ),
                              Container(
                                decoration: BoxDecoration(
                                  gradient: LinearGradient(
                                    colors: [Colors.black.withOpacity(0.5), Colors.transparent],
                                    begin: Alignment.bottomCenter,
                                    end: Alignment.topCenter,
                                  ),
                                ),
                              ),
                              Positioned(
                                bottom: 20,
                                left: 20,
                                child: Text(
                                  item['title']!,
                                  style: TextStyle(
                                    color: Colors.white,
                                    fontSize: 20,
                                    fontWeight: FontWeight.bold,
                                    shadows: [Shadow(color: Colors.black87, blurRadius: 6)],
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    );
                  }).toList(),
                ),
              ),

              // Dịch vụ nổi bật
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                child: Text(
                  "Dịch vụ nổi bật",
                  style: TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.w800,
                    color: Colors.blueGrey[900],
                    letterSpacing: 0.5,
                  ),
                ),
              ),
              Container(
                height: 160,
                padding: EdgeInsets.symmetric(horizontal: 8),
                child: ListView.builder(
                  scrollDirection: Axis.horizontal,
                  physics: BouncingScrollPhysics(),
                  itemCount: services.length,
                  itemBuilder: (context, index) {
                    return Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 8),
                      child: GestureDetector(
                        onTap: () {
                          ScaffoldMessenger.of(context).showSnackBar(
                            SnackBar(content: Text("Đã chọn ${services[index]['name']}")),
                          );
                        },
                        child: Card(
                          elevation: 8,
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
                          child: Container(
                            width: 120,
                            padding: EdgeInsets.all(12),
                            decoration: BoxDecoration(
                              gradient: LinearGradient(
                                colors: [Colors.white, Colors.grey[100]!],
                                begin: Alignment.topLeft,
                                end: Alignment.bottomRight,
                              ),
                              borderRadius: BorderRadius.circular(20),
                            ),
                            child: Column(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                ClipRRect(
                                  borderRadius: BorderRadius.circular(16),
                                  child: Image.network(
                                    services[index]['image']!,
                                    height: 80,
                                    width: 80,
                                    fit: BoxFit.cover,
                                  ),
                                ),
                                SizedBox(height: 12),
                                Text(
                                  services[index]['name']!,
                                  style: TextStyle(
                                    fontSize: 15,
                                    fontWeight: FontWeight.w700,
                                    color: Colors.blueGrey[800],
                                  ),
                                  textAlign: TextAlign.center,
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),
                    );
                  },
                ),
              ),

              // Nhân viên
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                child: Text(
                  "Nhân viên",
                  style: TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.w800,
                    color: Colors.blueGrey[900],
                    letterSpacing: 0.5,
                  ),
                ),
              ),
              Container(
                height: 160,
                padding: EdgeInsets.symmetric(horizontal: 8),
                child: ListView.builder(
                  scrollDirection: Axis.horizontal,
                  physics: BouncingScrollPhysics(),
                  itemCount: employees.length,
                  itemBuilder: (context, index) {
                    return Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 8),
                      child: Column(
                        children: [
                          Stack(
                            children: [
                              CircleAvatar(
                                radius: 50,
                                backgroundImage: NetworkImage(employees[index]['image']!),
                                backgroundColor: Colors.grey[200],
                              ),
                              Positioned(
                                bottom: 6,
                                right: 6,
                                child: Container(
                                  width: 16,
                                  height: 16,
                                  decoration: BoxDecoration(
                                    color: Colors.greenAccent,
                                    shape: BoxShape.circle,
                                    border: Border.all(color: Colors.white, width: 2),
                                    boxShadow: [
                                      BoxShadow(
                                        color: Colors.black26,
                                        blurRadius: 4,
                                        offset: Offset(0, 2),
                                      ),
                                    ],
                                  ),
                                ),
                              ),
                            ],
                          ),
                          SizedBox(height: 12),
                          Text(
                            employees[index]['name']!,
                            style: TextStyle(
                              fontSize: 15,
                              fontWeight: FontWeight.w600,
                              color: Colors.blueGrey[700],
                            ),
                          ),
                        ],
                      ),
                    );
                  },
                ),
              ),

              // Tiện ích
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                child: Text(
                  "Tiện ích",
                  style: TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.w800,
                    color: Colors.blueGrey[900],
                    letterSpacing: 0.5,
                  ),
                ),
              ),
              Column(
                children: blocks.map((block) {
                  return Card(
                    margin: EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                    elevation: 6,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                    child: ListTile(
                      contentPadding: EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                      leading: CircleAvatar(
                        radius: 26,
                        backgroundColor: (block['color'] as Color).withOpacity(0.15),
                        child: Icon(
                          block['icon'] as IconData,
                          color: block['color'] as Color,
                          size: 30,
                        ),
                      ),
                      title: Text(
                        block['title'] as String,
                        style: TextStyle(
                          fontSize: 17,
                          fontWeight: FontWeight.w700,
                          color: Colors.blueGrey[800],
                        ),
                      ),
                      trailing: Icon(
                        Icons.arrow_forward_ios,
                        size: 20,
                        color: Colors.grey[400],
                      ),
                      onTap: () {
                        ScaffoldMessenger.of(context).showSnackBar(
                          SnackBar(content: Text("Đã chọn ${block['title']}")),
                        );
                      },
                    ),
                  );
                }).toList(),
              ),
              SizedBox(height: 30),
            ],
          ),
        ),
      ),
      bottomNavigationBar: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          boxShadow: [
            BoxShadow(
              color: Colors.black12,
              blurRadius: 10,
              offset: Offset(0, -4),
            ),
          ],
        ),
        child: BottomNavigationBar(
          backgroundColor: Colors.transparent,
          elevation: 0,
          selectedItemColor: primaryColor,
          unselectedItemColor: Colors.grey[500],
          selectedLabelStyle: TextStyle(fontWeight: FontWeight.w700, fontSize: 12),
          unselectedLabelStyle: TextStyle(fontWeight: FontWeight.w500, fontSize: 12),
          showUnselectedLabels: true,
          items: [
            BottomNavigationBarItem(
              icon: Icon(Icons.home, size: 28),
              label: 'Trang chủ',
            ),
            BottomNavigationBarItem(
              icon: Icon(Icons.calendar_today, size: 28),
              label: 'Lịch',
            ),
            BottomNavigationBarItem(
              icon: Icon(Icons.person, size: 28),
              label: 'Tài khoản',
            ),
          ],
          currentIndex: 0,
          onTap: (index) {
            if (index == 2) {
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(content: Text("Chào ${user?.name ?? 'Người dùng'}")),
              );
            }
          },
        ),
      ),
    );
  }
}
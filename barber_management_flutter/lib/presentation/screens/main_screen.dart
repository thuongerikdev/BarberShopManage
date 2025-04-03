// import 'package:carousel_slider/carousel_slider.dart' as carousel;
// import 'package:flutter/material.dart';
// import 'package:provider/provider.dart';
// import '../providers/auth_provider.dart';
// import 'login_screen.dart';
// import 'BookingScreen.dart'; // Thêm màn hình đặt lịch

// void main() {
//   runApp(MyApp());
// }

// class MyApp extends StatelessWidget {
//   @override
//   Widget build(BuildContext context) {
//     return MaterialApp(
//       title: 'Barber Booking',
//       theme: ThemeData(
//         primaryColor: Color(0xFF1E88E5),
//         fontFamily: 'Poppins',
//       ),
//       home: MainScreen(),
//     );
//   }
// }

// class MainScreen extends StatefulWidget {
//   @override
//   _MainScreenState createState() => _MainScreenState();
// }

// class _MainScreenState extends State<MainScreen> with SingleTickerProviderStateMixin {
//   late AnimationController _controller;
//   late Animation<double> _fadeAnimation;
//   int _selectedIndex = 0; // Quản lý vị trí của BottomNavigationBar

//   @override
//   void initState() {
//     super.initState();
//     _controller = AnimationController(
//       duration: const Duration(milliseconds: 1200),
//       vsync: this,
//     );
//     _fadeAnimation = CurvedAnimation(parent: _controller, curve: Curves.easeInOut);
//     _controller.forward();
//   }

//   @override
//   void dispose() {
//     _controller.dispose();
//     super.dispose();
//   }

//   // Dữ liệu giả lập (giữ nguyên như bạn yêu cầu)
//   final List<Map<String, String>> sliderItems = [
//     {'image': '../../../image/Slider/slider1.jpg', 'title': 'Ưu đãi 50% Cắt tóc'},
//     {'image': '../../../image/Slider/slider2.jpg', 'title': 'Nhuộm tóc chuyên nghiệp'},
//     {'image': '../../../image/Slider/slider3.jpg', 'title': 'Massage thư giãn'},
//   ];

//   final List<Map<String, String>> services = [
//     {'name': 'Cắt tóc', 'image': '../../../image/Service/CuttingHair.jpg'},
//     {'name': 'Nhuộm tóc', 'image': '../../../image/Service/Massage.jpg'},
//     {'name': 'Massage', 'image': '../../../image/Service/HairDying.jpg'},
//   ];

//   final List<Map<String, String>> employees = [
//     {'name': 'Nguyễn Văn A', 'image': '../../../image/Employee/emp1.jpg'},
//     {'name': 'Trần Thị B', 'image': '../../../image/Employee/emp2.jpg'},
//     {'name': 'Lê Văn C', 'image': '../../../image/Employee/emp3.jpg'},
//     {'name': 'Lê Văn C', 'image': '../../../image/Employee/emp3.jpg'},
//   ];

//   final List<Map<String, dynamic>> blocks = [
//     {'title': 'Lịch sử đặt lịch', 'icon': Icons.history, 'color': Colors.blue},
//     {'title': 'Ưu đãi', 'icon': Icons.local_offer, 'color': Colors.orange},
//     {'title': 'Hỗ trợ', 'icon': Icons.support_agent, 'color': Colors.green},
//   ];

//   // Bảng màu chuyên nghiệp
//   static const primaryColor = Color(0xFF1E88E5);
//   static const backgroundColor = Color(0xFFF8FAFC);
//   static const textColor = Color(0xFF263238);

//   // Danh sách các trang
//   List<Widget> _pages(BuildContext context) => [
//         _buildHomeContent(context), // Trang chủ
//         BookingScreen(), // Trang đặt lịch
//         _buildProfileContent(context), // Trang tài khoản
//       ];

//   void _onItemTapped(int index) {
//     setState(() {
//       _selectedIndex = index;
//     });
//   }

//   @override
//   Widget build(BuildContext context) {
//     final authProvider = Provider.of<AuthProvider>(context);
//     final user = authProvider.user;

//     return Scaffold(
//       backgroundColor: backgroundColor,
//       appBar: AppBar(
//         elevation: 0,
//         backgroundColor: Colors.transparent,
//         flexibleSpace: Container(
//           decoration: BoxDecoration(
//             gradient: LinearGradient(
//               colors: [primaryColor, primaryColor.withOpacity(0.85)],
//               begin: Alignment.topLeft,
//               end: Alignment.bottomRight,
//             ),
//           ),
//         ),
//         title: Row(
//           children: [
//             Icon(Icons.cut, color: Colors.white, size: 30),
//             SizedBox(width: 10),
//             Text(
//               "Barber Booking",
//               style: TextStyle(
//                 fontFamily: 'Poppins',
//                 fontWeight: FontWeight.w700,
//                 fontSize: 26,
//                 color: Colors.white,
//                 letterSpacing: 1.5,
//               ),
//             ),
//           ],
//         ),
//         actions: [
//           IconButton(
//             icon: Icon(Icons.logout, color: Colors.white, size: 28),
//             onPressed: () {
//               authProvider.logout();
//               Navigator.pushReplacement(
//                 context,
//                 MaterialPageRoute(builder: (context) => LoginScreen()),
//               );
//             },
//           ),
//         ],
//       ),
//       body: IndexedStack(
//         index: _selectedIndex,
//         children: _pages(context),
//       ),
//       floatingActionButton: _selectedIndex == 0 // Chỉ hiển thị FAB ở Trang chủ
//           ? FloatingActionButton(
//               onPressed: () {
//                 Navigator.push(
//                   context,
//                   MaterialPageRoute(builder: (context) => BookingScreen()),
//                 );
//               },
//               backgroundColor: primaryColor,
//               elevation: 8,
//               tooltip: 'Đặt lịch ngay',
//               child: Icon(Icons.calendar_today, size: 28, color: Colors.white),
//             )
//           : null,
//       floatingActionButtonLocation: FloatingActionButtonLocation.endFloat, // Di chuyển FAB sang góc dưới bên phải
//       bottomNavigationBar: Container(
//         decoration: BoxDecoration(
//           color: Colors.white,
//           boxShadow: [
//             BoxShadow(
//               color: Colors.black12,
//               blurRadius: 12,
//               offset: Offset(0, -6),
//             ),
//           ],
//           borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
//         ),
//         child: BottomNavigationBar(
//           backgroundColor: Colors.transparent,
//           elevation: 0,
//           selectedItemColor: primaryColor,
//           unselectedItemColor: Colors.grey[600],
//           selectedLabelStyle: TextStyle(fontWeight: FontWeight.w700, fontSize: 13),
//           unselectedLabelStyle: TextStyle(fontWeight: FontWeight.w500, fontSize: 13),
//           showUnselectedLabels: true,
//           type: BottomNavigationBarType.fixed,
//           items: [
//             BottomNavigationBarItem(
//               icon: Icon(Icons.home, size: 30),
//               label: 'Trang chủ',
//             ),
//             BottomNavigationBarItem(
//               icon: Icon(Icons.calendar_today, size: 30),
//               label: 'Lịch',
//             ),
//             BottomNavigationBarItem(
//               icon: Icon(Icons.person, size: 30),
//               label: 'Tài khoản',
//             ),
//           ],
//           currentIndex: _selectedIndex,
//           onTap: _onItemTapped,
//         ),
//       ),
//     );
//   }

//   // Nội dung Trang chủ
//   Widget _buildHomeContent(BuildContext context) {
//     return FadeTransition(
//       opacity: _fadeAnimation,
//       child: SingleChildScrollView(
//         physics: BouncingScrollPhysics(),
//         child: Column(
//           crossAxisAlignment: CrossAxisAlignment.start,
//           children: [
//             // Slider khuyến mãi
//             Container(
//               height: 260,
//               margin: EdgeInsets.only(top: 20, bottom: 16),
//               child: carousel.CarouselSlider(
//                 options: carousel.CarouselOptions(
//                   autoPlay: true,
//                   autoPlayInterval: Duration(seconds: 4),
//                   aspectRatio: 16 / 9,
//                   enlargeCenterPage: true,
//                   viewportFraction: 0.85,
//                   enlargeStrategy: carousel.CenterPageEnlargeStrategy.height,
//                 ),
//                 items: sliderItems.map((item) {
//                   return GestureDetector(
//                     onTap: () => _showSnackBar(context, item['title']!),
//                     child: Container(
//                       margin: EdgeInsets.symmetric(horizontal: 10),
//                       decoration: BoxDecoration(
//                         borderRadius: BorderRadius.circular(30),
//                         boxShadow: [
//                           BoxShadow(
//                             color: Colors.black.withOpacity(0.2),
//                             blurRadius: 15,
//                             offset: Offset(0, 8),
//                           ),
//                         ],
//                       ),
//                       child: ClipRRect(
//                         borderRadius: BorderRadius.circular(30),
//                         child: Stack(
//                           fit: StackFit.expand,
//                           children: [
//                             Image.network(
//                               item['image']!,
//                               fit: BoxFit.cover,
//                               loadingBuilder: (context, child, loadingProgress) {
//                                 if (loadingProgress == null) return child;
//                                 return Center(
//                                   child: CircularProgressIndicator(
//                                     valueColor: AlwaysStoppedAnimation<Color>(primaryColor),
//                                   ),
//                                 );
//                               },
//                             ),
//                             Container(
//                               decoration: BoxDecoration(
//                                 gradient: LinearGradient(
//                                   colors: [Colors.black.withOpacity(0.6), Colors.transparent],
//                                   begin: Alignment.bottomCenter,
//                                   end: Alignment.topCenter,
//                                 ),
//                               ),
//                             ),
//                             Positioned(
//                               bottom: 25,
//                               left: 25,
//                               child: Text(
//                                 item['title']!,
//                                 style: TextStyle(
//                                   color: Colors.white,
//                                   fontSize: 22,
//                                   fontWeight: FontWeight.w700,
//                                   fontFamily: 'Poppins',
//                                   shadows: [Shadow(color: Colors.black87, blurRadius: 8)],
//                                 ),
//                               ),
//                             ),
//                           ],
//                         ),
//                       ),
//                     ),
//                   );
//                 }).toList(),
//               ),
//             ),

//             // Dịch vụ nổi bật
//             _buildSectionTitle(context, "Dịch vụ nổi bật"),
//             Container(
//               height: 180,
//               padding: EdgeInsets.symmetric(horizontal: 16),
//               child: ListView.builder(
//                 scrollDirection: Axis.horizontal,
//                 physics: BouncingScrollPhysics(),
//                 itemCount: services.length,
//                 itemBuilder: (context, index) {
//                   return Padding(
//                     padding: const EdgeInsets.only(right: 16),
//                     child: GestureDetector(
//                       onTap: () => _showSnackBar(context, services[index]['name']!),
//                       child: Card(
//                         elevation: 10,
//                         shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(25)),
//                         child: Container(
//                           width: 130,
//                           padding: EdgeInsets.all(16),
//                           decoration: BoxDecoration(
//                             gradient: LinearGradient(
//                               colors: [Colors.white, backgroundColor],
//                               begin: Alignment.topLeft,
//                               end: Alignment.bottomRight,
//                             ),
//                             borderRadius: BorderRadius.circular(25),
//                           ),
//                           child: Column(
//                             mainAxisAlignment: MainAxisAlignment.center,
//                             children: [
//                               ClipRRect(
//                                 borderRadius: BorderRadius.circular(20),
//                                 child: Image.network(
//                                   services[index]['image']!,
//                                   height: 90,
//                                   width: 90,
//                                   fit: BoxFit.cover,
//                                 ),
//                               ),
//                               SizedBox(height: 12),
//                               Text(
//                                 services[index]['name']!,
//                                 style: TextStyle(
//                                   fontSize: 16,
//                                   fontWeight: FontWeight.w600,
//                                   color: textColor,
//                                   fontFamily: 'Poppins',
//                                 ),
//                                 textAlign: TextAlign.center,
//                               ),
//                             ],
//                           ),
//                         ),
//                       ),
//                     ),
//                   );
//                 },
//               ),
//             ),

//             // Nhân viên
//             _buildSectionTitle(context, "Nhân viên"),
//             Container(
//               height: 180,
//               padding: EdgeInsets.symmetric(horizontal: 16),
//               child: ListView.builder(
//                 scrollDirection: Axis.horizontal,
//                 physics: BouncingScrollPhysics(),
//                 itemCount: employees.length,
//                 itemBuilder: (context, index) {
//                   return Padding(
//                     padding: const EdgeInsets.only(right: 16),
//                     child: Column(
//                       children: [
//                         Stack(
//                           children: [
//                             CircleAvatar(
//                               radius: 55,
//                               backgroundImage: NetworkImage(employees[index]['image']!),
//                               backgroundColor: Colors.grey[200],
//                             ),
//                             Positioned(
//                               bottom: 8,
//                               right: 8,
//                               child: Container(
//                                 width: 18,
//                                 height: 18,
//                                 decoration: BoxDecoration(
//                                   color: Colors.greenAccent,
//                                   shape: BoxShape.circle,
//                                   border: Border.all(color: Colors.white, width: 2.5),
//                                   boxShadow: [
//                                     BoxShadow(
//                                       color: Colors.black26,
//                                       blurRadius: 6,
//                                       offset: Offset(0, 2),
//                                     ),
//                                   ],
//                                 ),
//                               ),
//                             ),
//                           ],
//                         ),
//                         SizedBox(height: 12),
//                         Text(
//                           employees[index]['name']!,
//                           style: TextStyle(
//                             fontSize: 16,
//                             fontWeight: FontWeight.w600,
//                             color: textColor,
//                             fontFamily: 'Poppins',
//                           ),
//                         ),
//                       ],
//                     ),
//                   );
//                 },
//               ),
//             ),

//             // Tiện ích
//             _buildSectionTitle(context, "Tiện ích"),
//             Column(
//               children: blocks.map((block) {
//                 return Card(
//                   margin: EdgeInsets.symmetric(horizontal: 16, vertical: 10),
//                   elevation: 8,
//                   shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
//                   child: ListTile(
//                     contentPadding: EdgeInsets.symmetric(horizontal: 20, vertical: 12),
//                     leading: CircleAvatar(
//                       radius: 28,
//                       backgroundColor: (block['color'] as Color).withOpacity(0.1),
//                       child: Icon(
//                         block['icon'] as IconData,
//                         color: block['color'] as Color,
//                         size: 32,
//                       ),
//                     ),
//                     title: Text(
//                       block['title'] as String,
//                       style: TextStyle(
//                         fontSize: 18,
//                         fontWeight: FontWeight.w600,
//                         color: textColor,
//                         fontFamily: 'Poppins',
//                       ),
//                     ),
//                     trailing: Icon(
//                       Icons.arrow_forward_ios,
//                       size: 22,
//                       color: Colors.grey[500],
//                     ),
//                     onTap: () => _showSnackBar(context, block['title'] as String),
//                   ),
//                 );
//               }).toList(),
//             ),
//             SizedBox(height: 40),
//           ],
//         ),
//       ),
//     );
//   }

//   // Nội dung Trang tài khoản (tạm thời)
//   Widget _buildProfileContent(BuildContext context) {
//     final authProvider = Provider.of<AuthProvider>(context);
//     final user = authProvider.user;
//     return Center(
//       child: Column(
//         mainAxisAlignment: MainAxisAlignment.center,
//         children: [
//           CircleAvatar(
//             radius: 50,
//             backgroundColor: primaryColor,
//             child: Icon(Icons.person, size: 60, color: Colors.white),
//           ),
//           SizedBox(height: 20),
//           Text(
//             "Chào ${user?.name ?? 'Người dùng'}",
//             style: TextStyle(
//               fontSize: 24,
//               fontWeight: FontWeight.w700,
//               color: textColor,
//               fontFamily: 'Poppins',
//             ),
//           ),
//           SizedBox(height: 10),
//           ElevatedButton(
//             onPressed: () {
//               authProvider.logout();
//               Navigator.pushReplacement(
//                 context,
//                 MaterialPageRoute(builder: (context) => LoginScreen()),
//               );
//             },
//             style: ElevatedButton.styleFrom(
//               backgroundColor: primaryColor,
//               padding: EdgeInsets.symmetric(horizontal: 30, vertical: 15),
//               shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
//             ),
//             child: Text(
//               "Đăng xuất",
//               style: TextStyle(
//                 fontSize: 16,
//                 fontWeight: FontWeight.w600,
//                 color: Colors.white,
//                 fontFamily: 'Poppins',
//               ),
//             ),
//           ),
//         ],
//       ),
//     );
//   }

//   Widget _buildSectionTitle(BuildContext context, String title) {
//     return Padding(
//       padding: const EdgeInsets.fromLTRB(16, 20, 16, 12),
//       child: Text(
//         title,
//         style: TextStyle(
//           fontSize: 24,
//           fontWeight: FontWeight.w700,
//           color: textColor,
//           fontFamily: 'Poppins',
//           letterSpacing: 0.8,
//         ),
//       ),
//     );
//   }

//   void _showSnackBar(BuildContext context, String message) {
//     ScaffoldMessenger.of(context).showSnackBar(
//       SnackBar(
//         content: Text(
//           "Đã chọn $message",
//           style: TextStyle(fontFamily: 'Poppins', fontWeight: FontWeight.w500),
//         ),
//         behavior: SnackBarBehavior.floating,
//         shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
//         backgroundColor: primaryColor,
//         duration: Duration(seconds: 2),
//       ),
//     );
//   }
// }
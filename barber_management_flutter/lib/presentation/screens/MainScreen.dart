import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:barbermanagemobile/presentation/screens/auth/LoginScreen.dart';
import 'package:barbermanagemobile/presentation/screens/HomeScreen.dart';
import 'package:barbermanagemobile/presentation/screens/social/BlogScreen.dart';
import 'package:barbermanagemobile/presentation/screens/auth/ProfileScreen.dart';
import 'package:barbermanagemobile/presentation/screens/booking/BookingScreen.dart';
import 'package:url_launcher/url_launcher.dart';

class MainScreen extends StatefulWidget {
  const MainScreen({super.key});

  @override
  _MainScreenState createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> with SingleTickerProviderStateMixin {
  late AnimationController _controller;
  int _selectedIndex = 0;

  @override
  void initState() {
    super.initState();
    _controller = AnimationController(
      duration: const Duration(milliseconds: 1200),
      vsync: this,
    );
    _controller.forward();
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const accentColor = Color(0xFF8D6E63);

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });
    _controller.forward(from: 0); // Restart the fade animation for smooth transition
  }

  final List<Widget> _pages = [];

  // Function to show confirmation dialog before launching URL
  Future<bool?> _showSupportConfirmationDialog() async {
    return showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: primaryColor,
        title: Text(
          'Liên hệ hỗ trợ',
          style: TextStyle(
            color: Colors.white,
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
          ),
        ),
        content: Text(
          'Bạn có muốn mở trang Facebook hỗ trợ không?',
          style: TextStyle(
            color: Colors.grey[300],
            fontFamily: 'Poppins',
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: Text(
              'Hủy',
              style: TextStyle(
                color: accentColor,
                fontFamily: 'Poppins',
              ),
            ),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            child: Text(
              'Mở',
              style: TextStyle(
                color: Colors.green,
                fontFamily: 'Poppins',
              ),
            ),
          ),
        ],
      ),
    );
  }

  // Function to launch the Facebook URL
  Future<void> _launchFacebookURL() async {
    const String facebookUrl = 'https://www.facebook.com/share/15eArMEavv/';
    final Uri uri = Uri.parse(facebookUrl);

    // Show confirmation dialog
    final confirm = await _showSupportConfirmationDialog();
    if (confirm != true) return;

    try {
      print('Launching URL: $uri'); // Debug log
      if (kIsWeb) {
        if (!await launchUrl(uri, webOnlyWindowName: '_self')) {
          throw Exception('Không thể mở liên kết Facebook trên web');
        }
      } else {
        if (!await launchUrl(
          uri,
          mode: LaunchMode.externalApplication,
          webViewConfiguration: const WebViewConfiguration(
            enableJavaScript: true,
            enableDomStorage: true,
          ),
        )) {
          throw Exception('Không thể mở liên kết Facebook trên thiết bị');
        }
        // Add delay to prevent immediate focus return
        await Future.delayed(const Duration(milliseconds: 500));
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Lỗi khi mở liên kết Facebook: $e')),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    // Initialize _pages here to ensure _onItemTapped is available
    _pages.clear(); // Clear to avoid duplicates
    _pages.addAll([
      const HomeScreen(),
      const BlogScreen(),
      BookingScreen(onBack: () => _onItemTapped(0)), // Pass the callback to return to HomeScreen
      const ProfileScreen(),
    ]);

    final authProvider = Provider.of<AuthProvider>(context);

    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: Colors.transparent,
        flexibleSpace: Container(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [primaryColor, primaryColor.withOpacity(0.85)],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
        ),
        title: Row(
          children: [
            Icon(Icons.cut, color: Colors.white, size: 30),
            SizedBox(width: 10),
            Text(
              "Barber Booking",
              style: TextStyle(
                fontFamily: 'Poppins',
                fontWeight: FontWeight.w700,
                fontSize: 26,
                color: Colors.white,
                letterSpacing: 1.5,
              ),
            ),
          ],
        ),
        actions: [
          IconButton(
            icon: Icon(Icons.logout, color: Colors.white, size: 28),
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
      body: IndexedStack( // Removed FadeTransition to avoid focus issues
        index: _selectedIndex,
        children: _pages,
      ),
      floatingActionButton: _selectedIndex == 0
          ? FloatingActionButton(
              onPressed: _launchFacebookURL, // Updated to launch Facebook URL
              backgroundColor: accentColor,
              elevation: 8,
              tooltip: 'Hỗ trợ',
              child: Icon(Icons.support_agent, size: 28, color: Colors.white),
            )
          : null,
      floatingActionButtonLocation: FloatingActionButtonLocation.endFloat,
      bottomNavigationBar: Container(
        decoration: BoxDecoration(
          color: Color(0xFF2C1E1B),
          boxShadow: [
            BoxShadow(
              color: Colors.black26,
              blurRadius: 12,
              offset: Offset(0, -6),
            ),
          ],
          borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
        ),
        child: BottomNavigationBar(
          backgroundColor: Colors.transparent,
          elevation: 0,
          selectedItemColor: accentColor,
          unselectedItemColor: Colors.grey[400],
          selectedLabelStyle: TextStyle(fontWeight: FontWeight.w700, fontSize: 13),
          unselectedLabelStyle: TextStyle(fontWeight: FontWeight.w500, fontSize: 13),
          showUnselectedLabels: true,
          type: BottomNavigationBarType.fixed,
          items: [
            BottomNavigationBarItem(
              icon: Icon(Icons.home, size: 30),
              label: 'Trang chủ',
            ),
            BottomNavigationBarItem(
              icon: Icon(Icons.article, size: 30),
              label: 'Blog',
            ),
            BottomNavigationBarItem(
              icon: Icon(Icons.calendar_today, size: 30),
              label: 'Đặt lịch',
            ),
            BottomNavigationBarItem(
              icon: Icon(Icons.person, size: 30),
              label: 'Tài khoản',
            ),
          ],
          currentIndex: _selectedIndex,
          onTap: _onItemTapped,
        ),
      ),
    );
  }
}
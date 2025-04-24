import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:barbermanagemobile/presentation/screens/LoginScreen.dart';
import 'package:barbermanagemobile/presentation/screens/HomeScreen.dart';
import 'package:barbermanagemobile/presentation/screens/BlogScreen.dart';
import 'package:barbermanagemobile/presentation/screens/ProfileScreen.dart';
import 'package:barbermanagemobile/presentation/screens/BookingScreen.dart';

class MainScreen extends StatefulWidget {
  const MainScreen({super.key});

  @override
  _MainScreenState createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> with SingleTickerProviderStateMixin {
  late AnimationController _controller;
  late Animation<double> _fadeAnimation;
  int _selectedIndex = 0;

  @override
  void initState() {
    super.initState();
    _controller = AnimationController(
      duration: const Duration(milliseconds: 1200),
      vsync: this,
    );
    _fadeAnimation = CurvedAnimation(parent: _controller, curve: Curves.easeInOut);
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
      body: FadeTransition(
        opacity: _fadeAnimation,
        child: IndexedStack(
          index: _selectedIndex,
          children: _pages,
        ),
      ),
      floatingActionButton: _selectedIndex == 0
          ? FloatingActionButton(
              onPressed: () {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text('Đang liên hệ hỗ trợ...')),
                );
              },
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
import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/injection.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:barbermanagemobile/presentation/screens/LoginScreen.dart';
import 'package:barbermanagemobile/presentation/screens/MainScreen.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();

  // Load .env file
  await dotenv.load(fileName: "assets/.env");

  // Initialize dependency injection
  setupDependencies();
  verifyDependencies();

  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider(
          create: (_) => getIt<AuthProvider>(),
        ),
      ],
      child: MyApp(),
    ),
  );
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Barber Manage',
      theme: ThemeData(
        fontFamily: 'Poppins',
        primaryColor: Color(0xFF4E342E),
        scaffoldBackgroundColor: Color(0xFF212121),
        textTheme: TextTheme(
          bodyLarge: TextStyle(color: Color(0xFFEFEBE9)),
          bodyMedium: TextStyle(color: Color(0xFFEFEBE9)),
        ),
        colorScheme: ColorScheme.dark(
          primary: Color(0xFF4E342E),
          secondary: Color(0xFF8D6E63),
          surface: Color(0xFF212121),
          onSurface: Color(0xFFEFEBE9),
        ),
        useMaterial3: true,
      ),
      home: AuthWrapper(),
    );
  }
}

class AuthWrapper extends StatefulWidget {
  @override
  _AuthWrapperState createState() => _AuthWrapperState();
}

class _AuthWrapperState extends State<AuthWrapper> {
  bool _isLoading = true;
  bool _isLoggedIn = false;
  String? _errorMessage;

  @override
  void initState() {
    super.initState();
    _checkLoginStatus();
  }

  Future<void> _checkLoginStatus() async {
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    setState(() {
      _isLoading = true;
      _errorMessage = null;
    });
    try {
      _isLoggedIn = await authProvider.checkLoginStatus();
      print('AuthWrapper: Login status checked, isLoggedIn: $_isLoggedIn');
      if (!_isLoggedIn && authProvider.errorMessage != null) {
        _errorMessage = authProvider.errorMessage;
        print('AuthWrapper: Error message set: $_errorMessage');
      }
    } catch (e, stackTrace) {
      print('AuthWrapper: Error checking login status: $e, stack: $stackTrace');
      _errorMessage = 'Lỗi kiểm tra đăng nhập: $e';
      _isLoggedIn = false;
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Scaffold(
        backgroundColor: Color(0xFF212121),
        body: Center(
          child: CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation<Color>(Color(0xFF8D6E63)),
          ),
        ),
      );
    }
    if (_errorMessage != null) {
      return Scaffold(
        backgroundColor: Color(0xFF212121),
        body: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Text(
                _errorMessage!,
                style: TextStyle(
                  color: Colors.redAccent,
                  fontSize: 16,
                  fontFamily: 'Poppins',
                ),
                textAlign: TextAlign.center,
              ),
              SizedBox(height: 16),
              ElevatedButton(
                onPressed: _checkLoginStatus,
                style: ElevatedButton.styleFrom(
                  backgroundColor: Color(0xFF8D6E63),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(8),
                  ),
                ),
                child: Text(
                  'Thử lại',
                  style: TextStyle(
                    color: Color(0xFFEFEBE9),
                    fontFamily: 'Poppins',
                  ),
                ),
              ),
            ],
          ),
        ),
      );
    }
    print('AuthWrapper: Navigating to ${_isLoggedIn ? "MainScreen" : "LoginScreen"}');
    return _isLoggedIn ? MainScreen() : LoginScreen();
  }
}
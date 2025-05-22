import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/injection.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:barbermanagemobile/presentation/screens/auth/LoginScreen.dart';


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
      home: LoginScreen(), // Always start at LoginScreen
    );
  }
}
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'data/datasources/auth_remote_data_source.dart'; // Sửa 'datasourses' thành 'datasources'
import 'data/repositories/auth_repository_impl.dart';
import 'domain/usecases/login_use_case.dart';
import 'presentation/providers/auth_provider.dart';
import 'presentation/screens/login_screen.dart';

void main() {
  final authRemoteDataSource = AuthRemoteDataSourceImpl(); // Tạo instance
  final authRepository = AuthRepositoryImpl(authRemoteDataSource); // Tạo instance
  final loginUseCase = LoginUseCase(authRepository);

  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider(
          create: (_) => AuthProvider(loginUseCase),
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
      title: 'Flutter Clean Architecture',
      home: LoginScreen(),
    );
  }
}
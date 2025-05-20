import 'package:flutter/material.dart';
import 'package:barbermanagemobile/data/models/user_model.dart';
import 'package:barbermanagemobile/domain/usecases/login_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/register_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_user_by_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/update_avatar_use_case.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:get_it/get_it.dart';
import 'dart:convert';
import 'dart:io';

class AuthProvider with ChangeNotifier {
  final LoginUseCase loginUseCase;
  final RegisterUseCase registerUseCase;
  final FlutterSecureStorage _storage = const FlutterSecureStorage(
    aOptions: AndroidOptions(encryptedSharedPreferences: true),
    iOptions: IOSOptions(accessibility: KeychainAccessibility.first_unlock),
  );
  final GetUserByIDUseCase _getUserByIDUseCase = GetIt.instance<GetUserByIDUseCase>();
  final UpdateAvatarUseCase _updateAvatarUseCase = GetIt.instance<UpdateAvatarUseCase>();

  UserModel? _user;
  bool _isLoading = false;
  String? _errorMessage;

  AuthProvider(this.loginUseCase, this.registerUseCase);

  UserModel? get user => _user;
  bool get isLoading => _isLoading;
  String? get errorMessage => _errorMessage;

  // Check login status
  Future<bool> checkLoginStatus() async {
    _isLoading = true;
    _errorMessage = null;
    notifyListeners();
    try {
      final userJson = await _storage.read(key: 'user');
      print('AuthProvider: Raw user data from storage: $userJson');
      if (userJson != null && userJson.isNotEmpty) {
        final userMap = jsonDecode(userJson);
        print('AuthProvider: Decoded user map: $userMap');
        _user = UserModel.fromJson(userMap);
        print('AuthProvider: User loaded: ${_user?.toJson()}');
        return true;
      }
      print('AuthProvider: No user found in storage');
      return false;
    } catch (e, stackTrace) {
      print('AuthProvider: Error checking login status: $e, stack: $stackTrace');
      _errorMessage = 'Lỗi kiểm tra trạng thái đăng nhập: $e';
      return false;
    } finally {
      _isLoading = false;
      notifyListeners();
    }
  }

  Future<void> login(String email, String password) async {
    _isLoading = true;
    _errorMessage = null;
    notifyListeners();

    final result = await loginUseCase(email, password);
    result.fold(
      (failure) {
        _errorMessage = failure;
        _isLoading = false;
        notifyListeners();
        print('AuthProvider: Login failed: $failure');
      },
      (success) async {
        _user = success;
        try {
          final userJson = jsonEncode(success.toJson());
          await _storage.write(key: 'user', value: userJson);
          print('AuthProvider: User saved to storage: $userJson');
          // Verify write
          final storedUser = await _storage.read(key: 'user');
          print('AuthProvider: Verified stored user: $storedUser');
        } catch (e, stackTrace) {
          print('AuthProvider: Error saving user to storage: $e, stack: $stackTrace');
          _errorMessage = 'Lỗi lưu thông tin đăng nhập: $e';
        }
        _isLoading = false;
        notifyListeners();
      },
    );
  }

  Future<void> register({
    required int roleID,
    required String userName,
    required String password,
    required String email,
    required String phoneNumber,
    required String fullName,
    required String dateOfBirth,
    required String gender,
  }) async {
    _isLoading = true;
    _errorMessage = null;
    notifyListeners();

    final result = await registerUseCase(
      roleID: roleID,
      userName: userName,
      password: password,
      email: email,
      phoneNumber: phoneNumber,
      fullName: fullName,
      dateOfBirth: dateOfBirth,
      gender: gender,
    );
    result.fold(
      (failure) {
        _errorMessage = failure;
        _isLoading = false;
        notifyListeners();
        print('AuthProvider: Registration failed: $failure');
      },
      (success) async {
        _user = success;
        try {
          final userJson = jsonEncode(success.toJson());
          await _storage.write(key: 'user', value: userJson);
          print('AuthProvider: Registered user saved to storage: $userJson');
        } catch (e, stackTrace) {
          print('AuthProvider: Error saving registered user: $e, stack: $stackTrace');
          _errorMessage = 'Lỗi lưu thông tin đăng ký: $e';
        }
        _isLoading = false;
        notifyListeners();
      },
    );
  }

  Future<void> updateAvatar(File image) async {
    if (_user?.userId == null || int.tryParse(_user!.userId) == null) {
      _errorMessage = 'Không tìm thấy thông tin người dùng';
      notifyListeners();
      print('AuthProvider: Invalid userId for avatar update');
      return;
    }

    _isLoading = true;
    _errorMessage = null;
    notifyListeners();

    final userId = int.parse(_user!.userId);
    final result = await _updateAvatarUseCase.call(userId, image);
    result.fold(
      (failure) {
        _errorMessage = failure;
        _isLoading = false;
        notifyListeners();
        print('AuthProvider: Avatar update failed: $failure');
      },
      (_) async {
        final userResult = await _getUserByIDUseCase.call(userId);
        userResult.fold(
          (failure) {
            _errorMessage = 'Cập nhật ảnh thành công nhưng không thể làm mới dữ liệu: $failure';
            _isLoading = false;
            notifyListeners();
            print('AuthProvider: Failed to refresh user data: $failure');
          },
          (updatedUser) async {
            _user = updatedUser;
            try {
              final userJson = jsonEncode(updatedUser.toJson());
              await _storage.write(key: 'user', value: userJson);
              print('AuthProvider: Updated user saved to storage: $userJson');
            } catch (e, stackTrace) {
              print('AuthProvider: Error saving updated user: $e, stack: $stackTrace');
              _errorMessage = 'Lỗi lưu thông tin người dùng: $e';
            }
            _isLoading = false;
            notifyListeners();
          },
        );
      },
    );
  }

  Future<void> logout() async {
    _user = null;
    _errorMessage = null;
    try {
      await _storage.delete(key: 'user');
      print('AuthProvider: User data cleared from storage');
    } catch (e, stackTrace) {
      print('AuthProvider: Error clearing storage: $e, stack: $stackTrace');
    }
    notifyListeners();
  }
}
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/data/datasources/auth_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/booking_service_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/employee_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/slider_remote_data_source.dart';
import 'package:barbermanagemobile/data/repositories/auth_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/booking_service_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/employee_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/slider_repository_impl.dart';
import 'package:barbermanagemobile/domain/repositories/auth_repository.dart';
import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';
import 'package:barbermanagemobile/domain/repositories/employee_repository.dart';
import 'package:barbermanagemobile/domain/repositories/slider_repository.dart';
import 'package:barbermanagemobile/domain/usecases/create_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_services_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_slider_images_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/login_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';

final GetIt getIt = GetIt.instance;

void setupDependencies() {
  // Đăng ký DataSources
  getIt.registerLazySingleton<AuthRemoteDataSource>(
    () => AuthRemoteDataSourceImpl(),
  );
  getIt.registerLazySingleton<BookingServiceRemoteDataSource>(
    () => BookingServiceRemoteDataSource(),
  );
  getIt.registerLazySingleton<EmployeeRemoteDataSource>(
    () => EmployeeRemoteDataSource(),
  );
  getIt.registerLazySingleton<SliderRemoteDataSource>(
    () => SliderRemoteDataSource(),
  );

  // Đăng ký Repositories (Abstract class và Implementation)
  getIt.registerLazySingleton<AuthRepository>(
    () => AuthRepositoryImpl(getIt<AuthRemoteDataSource>()),
  );
  getIt.registerLazySingleton<BookingServiceRepository>(
    () => BookingServiceRepositoryImpl(getIt<BookingServiceRemoteDataSource>()),
  );
  getIt.registerLazySingleton<EmployeeRepository>(
    () => EmployeeRepositoryImpl(getIt<EmployeeRemoteDataSource>()),
  );
  getIt.registerLazySingleton<SliderRepository>(
    () => SliderRepositoryImpl(getIt<SliderRemoteDataSource>()),
  );

  // Đăng ký UseCases
  getIt.registerLazySingleton<LoginUseCase>(
    () => LoginUseCase(getIt<AuthRepository>()),
  );
  getIt.registerLazySingleton<CreateBookingOrderUseCase>(
    () => CreateBookingOrderUseCase(getIt<BookingServiceRepository>()),
  );
  getIt.registerLazySingleton<GetBookingServicesUseCase>(
    () => GetBookingServicesUseCase(getIt<BookingServiceRepository>()),
  );
  getIt.registerLazySingleton<GetEmployeesUseCase>(
    () => GetEmployeesUseCase(getIt<EmployeeRepository>()),
  );
  getIt.registerLazySingleton<GetSliderImagesUseCase>(
    () => GetSliderImagesUseCase(getIt<SliderRepository>()),
  );

  // Đăng ký Providers
  getIt.registerLazySingleton<AuthProvider>(
    () => AuthProvider(getIt<LoginUseCase>()),
  );
}
import 'package:barbermanagemobile/data/datasources/promotion_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/promotion.dart';
import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';



class PromotionRepositoryImpl implements PromotionRepository {
  final PromotionRemoteDataSource remoteDataSource;

  PromotionRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Promotion>> getAllPromoByCustomer(int customerId) async {
    try {
      final promotions = await remoteDataSource.getAllPromoByCustomer(customerId);
      return promotions;
    } catch (e) {
      throw Exception('Failed to fetch promotions: $e');
    }
  }

  @override
  Future<void> createCustomerPromotion(int customerId, int promoId, String status) async {
    try {
      await remoteDataSource.createCustomerPromotion(customerId, promoId, status);
    } catch (e) {
      throw Exception('Failed to create customer promotion: $e');
    }
  }
}
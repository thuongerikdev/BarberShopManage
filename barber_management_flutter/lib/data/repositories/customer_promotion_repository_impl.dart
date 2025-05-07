import 'package:barbermanagemobile/data/datasources/customer_promotion_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/customer_promotion.dart';
import 'package:barbermanagemobile/domain/repositories/customer_promotion_repository.dart';

class CustomerPromotionRepositoryImpl implements CustomerPromotionRepository {
  final CustomerPromotionRemoteDataSource remoteDataSource;

  CustomerPromotionRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<CustomerPromotion>> getCustomerPromotions(int customerId) async {
    try {
      final promotions = await remoteDataSource.getCustomerPromotions(customerId);
      return promotions;
    } catch (e) {
      throw Exception('Failed to fetch customer promotions: $e');
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
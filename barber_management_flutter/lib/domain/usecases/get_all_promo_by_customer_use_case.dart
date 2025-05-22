import 'package:barbermanagemobile/domain/entities/promotion.dart';
import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';

class GetAllPromoByCustomerUseCase {
  final PromotionRepository repository;

  GetAllPromoByCustomerUseCase(this.repository);

  Future<List<Promotion>> call(int customerId) async {
    return await repository.getAllPromoByCustomer(customerId);
  }
}